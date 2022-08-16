using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Mvc.ApiExplorer;

using Serilog;
using Serilog.Core;

namespace RPSSL.UI;

[ExcludeFromCodeCoverage]
public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        using var logger = CreateLogger(builder);

        try
        {
            logger.Information("Starting the web application...");

            await using var app = BuildWebApplication(builder);

            logger.Information("The web application is ready");

            await app.RunAsync();
        }
        catch (Exception exception)
        {
            logger.Fatal(exception, "An unhandled exception occured...");

            throw;
        }
    }

    private static Logger CreateLogger(WebApplicationBuilder builder)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog(logger, true);

        return logger;
    }

    private static WebApplication BuildWebApplication(WebApplicationBuilder builder)
    {
        var startup = new Startup(builder.Configuration);

        startup.ConfigureServices(builder.Services);

        var app = builder.Build();
        using var startupScope = app.Services.CreateScope();

        var apiVersionProvider = startupScope
            .ServiceProvider
            .GetRequiredService<IApiVersionDescriptionProvider>();

        startup.Configure(app, app.Environment, apiVersionProvider);

        return app;
    }
}
