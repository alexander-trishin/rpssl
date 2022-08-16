using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Mvc.ApiExplorer;

using Serilog;

namespace RPSSL.UI;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        try
        {
            Log.Information("Web application is starting...");

            var startup = new Startup(builder.Configuration);

            startup.ConfigureServices(builder.Services);

            var app = builder.Build();

            var env = app.Environment;
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            startup.Configure(app, env, provider);

            Log.Information("Web application is ready");

            await app.RunAsync();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "An unhandled exception occured...");

            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
