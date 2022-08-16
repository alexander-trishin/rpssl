using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace RPSSL.UI.Swagger;

[ExcludeFromCodeCoverage]
public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
        }

        var xmlFileName = Assembly.GetExecutingAssembly().GetName().Name;
        var xmlFilePath = Path.Combine(AppContext.BaseDirectory, $"{xmlFileName}.xml");

        options.IncludeXmlComments(xmlFilePath);
    }

    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = Constants.AppName,
            Description = "An API for 'Rock, Paper, Scissors, Lizard, Spock' game.",
            Version = description.ApiVersion.ToString(),
            Contact = new OpenApiContact
            {
                Name = "GitHub Repository",
                Url = new Uri("https://github.com/alexander-trishin/rpssl")
            },
            License = new OpenApiLicense
            {
                Name = "MIT License",
                Url = new Uri("https://github.com/alexander-trishin/rpssl/blob/master/LICENSE")
            }
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}
