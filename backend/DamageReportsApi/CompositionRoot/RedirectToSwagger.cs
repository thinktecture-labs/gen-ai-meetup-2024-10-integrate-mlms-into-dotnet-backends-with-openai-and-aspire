using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace DamageReportsApi.CompositionRoot;

public static class RedirectToSwagger
{
    public static void MapRedirectToSwagger(this WebApplication app) =>
        app.MapGet(
                "/",
                context =>
                {
                    context.Response.Redirect("/swagger");
                    return Task.CompletedTask;
                }
            )
           .ExcludeFromDescription();
}