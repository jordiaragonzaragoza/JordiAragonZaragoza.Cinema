namespace JordiAragon.Cinema.Presentation.WebApi.AssemblyConfiguration
{
    using System.Text.Json.Serialization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Quartz;

    public static class ConfigureServices
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<SerilogConsoleOptions>(configuration.GetSection(SerilogConsoleOptions.Section));

            serviceCollection.AddAutoMapper(WebApiAssemblyReference.Assembly);

            serviceCollection.AddCors(options =>
            {
                options.AddPolicy(
                    name: Constants.AllowAnyOriginPolicy,
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
            });

            serviceCollection
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;

                    // How to read api version.
                    // More info: https://github.com/dotnet/aspnet-api-versioning/wiki/API-Version-Reader
                    options.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader(),
                        ////new QueryStringApiVersionReader("api-version"),
                        ////new HeaderApiVersionReader("X-Version"),
                        new MediaTypeApiVersionReader("version"));
                });

            // Add ApiExplorer to discover versions
            serviceCollection.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            serviceCollection
                .AddControllers(options =>
                {
                    options.AllowEmptyInputInBodyModelBinding = true;
                })
                .AddApplicationPart(WebApiAssemblyReference.Assembly)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

                    // TODO: Review if there is some issue with System.Text.Json related with these settings
                    // https://github.com/dotnet/runtime/issues/1566
                    ////options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                    ////options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            serviceCollection.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddSwaggerGen();

            serviceCollection.AddQuartzHostedService();

            serviceCollection.AddHttpContextAccessor();

            serviceCollection.AddHealthChecks();

            return serviceCollection;
        }
    }
}