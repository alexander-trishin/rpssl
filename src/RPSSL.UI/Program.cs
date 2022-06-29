using System.Diagnostics.CodeAnalysis;

using Serilog;

namespace RPSSL.UI;

[ExcludeFromCodeCoverage]
public sealed class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        try
        {
            Log.Information("Web application is starting...");

            var startup = new Startup();

            startup.ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            startup.ConfigureApplication(app, app.Environment);
            startup.ConfigureEndpoints(app);

            Log.Information("Web application is ready!");

            app.Run();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Unhandled exception!");

            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
