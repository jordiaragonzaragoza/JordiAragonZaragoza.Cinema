namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Asp.Versioning.ApiExplorer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private const string BearerSchemeName = "Bearer";
        private const string OAuth2SchemeName = "oauth2";

        private readonly IApiVersionDescriptionProvider provider;
        private readonly IConfiguration configuration;

        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider provider,
            IConfiguration configuration)
        {
            this.provider = provider;
            this.configuration = configuration;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in this.provider.ApiVersionDescriptions.Reverse())
            {
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }

            // Alternativa: pegar el token manualmente.
            options.AddSecurityDefinition(BearerSchemeName, new OpenApiSecurityScheme()
            {
                Description = "Pega aquí el JWT (sin el prefijo 'Bearer').",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
            });

            options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement()
            {
                [new OpenApiSecuritySchemeReference(BearerSchemeName, doc)] = [],
            });

            // Login contra Keycloak (Authorization Code + PKCE).
            var authority = this.configuration["Authentication:Schemes:Bearer:Authority"];
            if (!string.IsNullOrWhiteSpace(authority))
            {
                var oidc = $"{authority.TrimEnd('/')}/protocol/openid-connect";

                options.AddSecurityDefinition(OAuth2SchemeName, new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        AuthorizationCode = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{oidc}/auth"),
                            TokenUrl = new Uri($"{oidc}/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                ["openid"] = "OpenID",
                                ["profile"] = "Profile",
                            },
                        },
                    },
                });

                options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement()
                {
                    [new OpenApiSecuritySchemeReference(OAuth2SchemeName, doc)] = ["openid"],
                });
            }

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
                Title = typeof(ApiQueryAssemblyReference).Namespace,
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