using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

using FluentValidation.AspNetCore;

using MediatR;

using MicroElements.Swashbuckle.FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Versioning;

using Polly;

using RPSSL.Application.Game;
using RPSSL.Infrastructure.Services;
using RPSSL.UI.Routing;
using RPSSL.UI.Swagger;

namespace RPSSL.UI;

[ExcludeFromCodeCoverage]
public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;

                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

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
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { UseCookies = false })
            .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(15),
                TimeSpan.FromSeconds(45)
            }));

        services.AddCors(cors =>
        {
            cors.AddPolicy(Constants.CorsPolicy.CodeChallenge, policy =>
            {
                policy
                    .WithOrigins(_configuration[Constants.Settings.CodeChallengeUrl])
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
            var httpClient = CreateHttpClient(provider, _configuration[Constants.Settings.CodeChallengeUrl]);

            return new RandomService(httpClient);
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseCors(Constants.CorsPolicy.CodeChallenge);

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName
                );
            }
        });

        app.UseEndpoints(endpoint =>
        {
            endpoint.MapControllers();
        });
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
