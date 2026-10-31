namespace JordiAragonZaragoza.Cinema.SharedKernel.Presentation.HttpRestfulApi
{
    using System;
    using System.Linq;
    using Asp.Versioning.ApiExplorer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public sealed class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private const string BearerSchemeName = "Bearer";
        private const string OAuth2SchemeName = "oauth2";

        private readonly IApiVersionDescriptionProvider provider;
        private readonly IConfiguration configuration;
        private readonly ApiOptions apiOptions;

        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider provider,
            IConfiguration configuration,
            ApiOptions apiOptions)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.apiOptions = apiOptions ?? throw new ArgumentNullException(nameof(apiOptions));
        }

        /// <summary>
        /// Configure each API discovered for Swagger Documentation.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(SwaggerGenOptions options)
        {
            ArgumentNullException.ThrowIfNull(options);

            // Add a swagger document for every API version discovered.
            foreach (var description in this.provider.ApiVersionDescriptions.Reverse())
            {
                options.SwaggerDoc(description.GroupName, this.CreateVersionInfo(description));
            }

            // Alternative: paste the token manually.
            options.AddSecurityDefinition(BearerSchemeName, new OpenApiSecurityScheme()
            {
                Description = "Paste the JWT here (without the 'Bearer' prefix).",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
            });

            options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement()
            {
                [new OpenApiSecuritySchemeReference(BearerSchemeName, doc)] = [],
            });

            // Login against the identity provider (Authorization Code + PKCE).
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
                            Scopes = this.apiOptions.SwaggerScopes.ToDictionary(scope => scope, scope => scope),
                        },
                    },
                });

                options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement()
                {
                    [new OpenApiSecuritySchemeReference(OAuth2SchemeName, doc)] = [.. this.apiOptions.SwaggerScopes],
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
        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = this.apiOptions.Title,
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
