using System;
using System.Threading.Tasks;
using Aspire.Hosting;
using Serilog;
using Shared.CompositionRoot;

namespace AspireAppHost;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        Log.Logger = Logging.CreateBootstrapLogger();
        try
        {
            await using var distributedApp = DistributedApplication
               .CreateBuilder(args)
               .Configure()
               .SetUpProjectStructure()
               .Build();

            await distributedApp.RunAsync();
            return 0;
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Could not run AspireAppHost");
            return 1;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}