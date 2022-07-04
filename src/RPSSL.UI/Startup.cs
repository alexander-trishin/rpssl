using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

using FluentValidation.AspNetCore;

using MediatR;

using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Mvc.ApplicationModels;

using Polly;

using RPSSL.Application.Game;
using RPSSL.Infrastructure.Services;
using RPSSL.UI.Routing;
using RPSSL.UI.Swagger;

namespace RPSSL.UI;

[ExcludeFromCodeCoverage]
public class Startup
{
    private const string CodeChallengeCorsPolicy = nameof(CodeChallengeCorsPolicy);

    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddControllersWithViews(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
            })
            .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>())
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

        services
            .AddHttpClient(string.Empty)
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler() { UseCookies = false })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(15),
                TimeSpan.FromSeconds(45),
            }));

        services.AddCors(cors =>
        {
            cors.AddPolicy(CodeChallengeCorsPolicy, policy =>
            {
                policy
                    .WithOrigins(configuration["CodeChallengeUrl"])
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        services.AddEndpointsApiExplorer();

        services
            .ConfigureOptions<ConfigureSwaggerGenOptions>()
            .AddSwaggerGen()
            .AddFluentValidationRulesToSwagger();

        services.AddMediatR(typeof(IRuleBook));

        services.AddTransient<IRuleBook, RuleBook>();

        services.AddTransient<IRandomService>(provider =>
        {
            var httpClient = CreateHttpClient(provider, configuration["CodeChallengeUrl"]);

            return new RandomService(httpClient);
        });
    }

    public void ConfigureApplication(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors(CodeChallengeCorsPolicy);
    }

    public void ConfigureEndpoints(IEndpointRouteBuilder endpoint)
    {
        endpoint.MapControllers();
        endpoint.MapSwagger();
    }

    private static HttpClient CreateHttpClient(IServiceProvider provider, string baseAddress)
    {
        var httpClient = provider
            .GetRequiredService<IHttpClientFactory>()
            .CreateClient();

        httpClient.BaseAddress = new Uri(baseAddress);

        return httpClient;
    }
}
