using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared;

namespace AiInformationExtractionApi.DatabaseAccess;

public static class DatabaseAccessModule
{
    public static WebApplicationBuilder AddDatabaseAccess(this WebApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<AiInformationExtractionDbContext>(Constants.AiInformationExtractionDatabaseName);
        return builder;
    }

    public static async Task ApplyDatabaseMigrationsAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AiInformationExtractionDbContext>();
        var executionStrategy = dbContext.Database.CreateExecutionStrategy();
        await executionStrategy.ExecuteAsync(dbContext, context => context.Database.MigrateAsync());
    }
}