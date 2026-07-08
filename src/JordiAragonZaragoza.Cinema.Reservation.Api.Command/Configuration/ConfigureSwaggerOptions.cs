namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Configuration
{
    using System;
    using System.Linq;
    using Asp.Versioning.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private const string SecuritySchemeName = "Bearer";
        private readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// Configure each API discovered for Swagger Documentation.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(SwaggerGenOptions options)
        {
            // add swagger document for every API version discovered
            foreach (var description in this.provider.ApiVersionDescriptions.Reverse())
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateVersionInfo(description));
            }

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });

            options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement()
                {
                    [new OpenApiSecuritySchemeReference(SecuritySchemeName, doc)] = [],
                });

            options.OperationFilter<AddRequiredHeadersOperationFilter>();

            options.CustomSchemaIds(type => type.FullName);
        }

        /// <summary>
        /// Configure Swagger Options. Inherited from the Interface.
        /// </summary>
        /// <param name="name">The name of the options instance being configured.</param>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(string? name, SwaggerGenOptions options)
        {
            this.Configure(options);
        }

        /// <summary>
        /// Create information about the version of the API.
        /// </summary>
        /// <param name="description">The description of the API version.</param>
        /// <returns>Information about the API.</returns>
        private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = typeof(ApiCommandAssemblyReference).Namespace,
                Version = description.ApiVersion.ToString(),
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated. Please use one of the new APIs available from the explorer.";
            }

            return info;
        }
    }
}