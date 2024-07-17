using Aspire.Hosting;
using Projects;
using Shared;
using Shared.CompositionRoot;

namespace AspireAppHost;

public static class CompositionRoot
{
    public static IDistributedApplicationBuilder Configure(this IDistributedApplicationBuilder builder)
    {
        builder.Services.AddDefaultLogging(builder.Configuration);
        return builder;
    }

    public static IDistributedApplicationBuilder SetUpProjectStructure(this IDistributedApplicationBuilder builder)
    {
        var gateway = builder.AddProject<Gateway>(Constants.GatewayName);
        var identityServer = builder.AddProject<IdentityServer>(Constants.IdentityServerName);
        identityServer.WithReference(gateway);

        var postgresServer = builder.AddPostgres(Constants.PostgresServerName);
        var damageReportsDatabase = postgresServer.AddDatabase(Constants.DamageReportsDatabaseName);
        var aiInformationExtractionDatabase = postgresServer.AddDatabase(Constants.AiInformationExtractionDatabaseName);

        var damageReportsApi = builder
           .AddProject<DamageReportsApi>(Constants.DamageReportsServiceName)
           .WithReference(damageReportsDatabase)
           .WithReference(identityServer);

        var aiInformationExtractionApi = builder
           .AddProject<AiInformationExtractionApi>(Constants.AiInformationExtractionServiceName)
           .WithReference(aiInformationExtractionDatabase)
           .WithReference(identityServer);

        gateway
           .WithReference(identityServer)
           .WithReference(damageReportsApi)
           .WithReference(aiInformationExtractionApi);

        return builder;
    }
}