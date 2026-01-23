namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Configuration
{
    using System.Text.Json.Serialization;
    using Asp.Versioning;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationHttpRestfulApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication();
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
                    options.DefaultApiVersion = new ApiVersion(2);
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
                    // Add ApiExplorer to discover versions
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
            services.AddHealthChecks();

            return services;
        }
    }
}