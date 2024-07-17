using Microsoft.Extensions.DependencyInjection;

namespace Shared.Paging;

public static class PagingModule
{
    public static IServiceCollection AddPagingSupport(this IServiceCollection services) =>
        services.AddSingleton<PagingParametersValidator>();
}