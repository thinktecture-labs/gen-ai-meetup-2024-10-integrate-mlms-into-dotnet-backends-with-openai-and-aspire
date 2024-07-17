using System;
using Gateway.Transformation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Shared.CompositionRoot;

namespace Gateway.CompositionRoot;

public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.ConfigureDefaultLogging();

        var services = builder.Services;
        services
           .AddServiceDiscovery()
           .ConfigureHttpClientDefaults(options => options.AddServiceDiscovery())
           .AddOpenTelemetryMetricsAndTracing(builder.Configuration)
           .AddDefaultHealthChecks();

        services
           .AddReverseProxy()
           .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
           .AddTransforms<AuthenticationTransformProvider>()
           .AddServiceDiscoveryDestinationResolver();

        services
           .AddAuthentication(
                options =>
                {
                    options.DefaultScheme = "Cookie";
                    options.DefaultChallengeScheme = "OIDC";
                    options.DefaultSignOutScheme = "OIDC";
                }
            )
           .AddCookie(
                "Cookie",
                options =>
                {
                    options.Cookie.Name = "aspire.ai.gateway";
                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                    options.SlidingExpiration = true;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.HttpOnly = true;
                }
            )
           .AddOpenIdConnect(
                "OIDC",
                options =>
                {
                    options.Authority = builder.Configuration["services:IdentityServer:https:0"];
                    options.ClientId = "spa";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";
                    options.ResponseMode = "query";
                    options.MapInboundClaims = false;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("webapi");
                    options.Scope.Add("offline_access");
                }
            );

        services.AddAuthorization();

        services
           .AddOpenIdConnectAccessTokenManagement()
           .AddDistributedMemoryCache();
        return builder;
    }
}