using System;
using System.Threading.Tasks;
using Gateway.CompositionRoot;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Shared.CompositionRoot;

namespace Gateway;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        Log.Logger = Logging.CreateBootstrapLogger();
        try
        {
            await using var app = WebApplication
               .CreateBuilder(args)
               .ConfigureServices()
               .Build()
               .ConfigureMiddleware();

            await app.RunAsync();
            return 0;
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Could not run Gateway");
            return 1;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }
}
