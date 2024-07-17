using Microsoft.Extensions.Configuration;

namespace AspireAppHost.IntegrationTests;

public static class TestSettingExtensions
{
    public static bool AreTestsEnabled(this IConfiguration configuration) =>
        configuration.GetValue<bool>("areTestsEnabled");
}