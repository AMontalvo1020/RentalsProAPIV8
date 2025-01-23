using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace RentalsProAPIV8
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        static string ProjectName => Assembly.GetExecutingAssembly().GetName().Name;
        readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
            => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName,
                                   new OpenApiInfo
                                   {
                                       Title = $"{ProjectName} {description.ApiVersion}",
                                       Version = description.ApiVersion.ToString()
                                   });
            }
        }
    }
}
