using System;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Testing;
using Duende.AccessTokenManagement;
using IdentityModel.Client;
using Light.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using Xunit;

namespace AspireAppHost.IntegrationTests;

// ReSharper disable once ClassNeverInstantiated.Global -- instantiated by xunit.net
public sealed class AspireFixture : IAsyncLifetime
{
    private DistributedApplication? _app;
    private ServiceProvider? _tokenManagementServiceProvider;

    public DistributedApplication App =>
        _app ?? throw new InvalidOperationException("The application is not initialized");

    private ServiceProvider TokenManagementServiceProvider =>
        _tokenManagementServiceProvider ??
        throw new InvalidOperationException("The token management service provider is not initialized");

    public async Task InitializeAsync()
    {
        if (!TestSettings.Configuration.AreTestsEnabled())
        {
            return;
        }

        var appBuilder = await DistributedApplicationTestingBuilder.CreateAsync<Projects.AspireAppHost>();
        var services = appBuilder.Services;
        services.ConfigureHttpClientDefaults(
            clientBuilder => { clientBuilder.ConfigureHttpClient(client => client.Timeout = TimeSpan.FromMinutes(2)); }
        );
        var app = _app = await appBuilder.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();
        await resourceNotificationService
           .WaitForResourceAsync(Constants.AiInformationExtractionServiceName, KnownResourceStates.Running)
           .WaitAsync(TimeSpan.FromSeconds(30));
        var tokenManagementServices = new ServiceCollection();
        tokenManagementServices
           .AddDistributedMemoryCache()
           .AddClientCredentialsTokenManagement()
           .AddClient(
                "integration-tests",
                client =>
                {
                    var identityServerUri = app.GetEndpoint(Constants.IdentityServerName);
                    client.TokenEndpoint = new Uri(identityServerUri, "/connect/token").ToString();
                    client.ClientId = "integration-tests";
                    client.ClientSecret = "password";
                    client.Scope = "webapi";
                }
            );
        _tokenManagementServiceProvider = tokenManagementServices.BuildServiceProvider();
    }

    public async Task DisposeAsync()
    {
        if (_tokenManagementServiceProvider is not null)
        {
            await _tokenManagementServiceProvider.DisposeAsync();
        }

        if (_app is not null)
        {
            await _app.DisposeAsync();
        }
    }

    public async Task<HttpClient> CreateAuthenticatedHttpClientAsync(string resourceName)
    {
        var httpClient = App.CreateHttpClient(resourceName);
        var token = await TokenManagementServiceProvider
           .GetRequiredService<IClientCredentialsTokenManagementService>()
           .GetAccessTokenAsync("integration-tests");
        if (token.AccessToken is null)
        {
            throw new AuthenticationException("Could not authenticate integration test with Client Credentials");
        }

        httpClient.SetBearerToken(token.AccessToken);
        return httpClient;
    }

    public static void SkipTestIfNecessary() => Skip.IfNot(TestSettings.Configuration.AreTestsEnabled());
}