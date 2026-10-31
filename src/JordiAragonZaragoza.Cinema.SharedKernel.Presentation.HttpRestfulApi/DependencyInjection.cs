namespace JordiAragonZaragoza.Cinema.SharedKernel.Presentation.HttpRestfulApi
{
    using System;
    using System.Text.Json.Serialization;
    using Asp.Versioning;
    using JordiAragonZaragoza.Cinema.SharedKernel;

    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationHttpRestfulApi(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment,
            ApiOptions apiOptions)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);
            ArgumentNullException.ThrowIfNull(environment);
            ArgumentNullException.ThrowIfNull(apiOptions);

            services.AddSingleton(apiOptions);

            // Single place to change when the identity provider changes (Entra ID in production).
            services.AddAuthentication()
                    .AddKeycloakJwtBearer(
                        serviceName: Constants.Keycloak,
                        realm: Constants.KeycloakRealm,
                        options =>
                        {
                            options.Audience = configuration["Authentication:Audience"] ?? Constants.KeycloakAudience;
                            options.MapInboundClaims = false;

                            // For development only - disable HTTPS metadata validation.
                            // In production, use explicit Authority configuration instead.
                            if (environment.IsDevelopment())
                            {
                                options.RequireHttpsMetadata = false;
                            }
                        });

            services.AddAuthorization();

            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: ConfigureCors.CorsPolicy,
                    policy: ConfigureCors.BuildPolicy(configuration));
            });

            services
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = apiOptions.DefaultApiVersion;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();

                    // How to read api version.
                    // More info: https://github.com/dotnet/aspnet-api-versioning/wiki/API-Version-Reader
                    /*options.ApiVersionReader = ApiVersionReader.Combine(
                         new UrlSegmentApiVersionReader(),
                         new QueryStringApiVersionReader("api-version"),
                         new HeaderApiVersionReader("X-Version"),
                         new MediaTypeApiVersionReader("version"));*/
                })
                .AddApiExplorer(setup =>
                {
                    // Add ApiExplorer to discover versions.
                    setup.GroupNameFormat = "'v'VVV";
                    setup.SubstituteApiVersionInUrl = true;
                });

            services
                .AddControllers(options =>
                {
                    options.AllowEmptyInputInBodyModelBinding = true;
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

                    // TODO: Review if there is some issue with System.Text.Json related with these settings
                    // https://github.com/dotnet/runtime/issues/1566
                    ////options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    ////options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options => options.EnableAnnotations());
            services.ConfigureOptions<ConfigureSwaggerOptions>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}