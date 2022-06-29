using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace RPSSL.UI.Swagger;

[ExcludeFromCodeCoverage]
public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = Constants.AppName,
            Description = "An API for 'Rock, Paper, Scissors, Lizard, Spock' game.",
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
        });

        var xmlFileName = Assembly.GetExecutingAssembly().GetName().Name;
        var xmlFilePath = Path.Combine(AppContext.BaseDirectory, $"{xmlFileName}.xml");

        options.IncludeXmlComments(xmlFilePath);
    }
}
