namespace JordiAragon.Cinemas.Ticketing.Common.Presentation.WebApi
{
    using System.Text.Json.Serialization;
    using FastEndpoints;
    using FastEndpoints.Swagger;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Versioning;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using Quartz;

    public static class ConfigureServices
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<SerilogConsoleOptions>(configuration.GetSection(SerilogConsoleOptions.Section));

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

            /*
            serviceCollection
                .AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(2, 0);
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
            }); */

            /*
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
                });*/

            serviceCollection.AddFastEndpoints();

            serviceCollection.AddAuthentication();

            ////serviceCollection.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            ////serviceCollection.AddEndpointsApiExplorer();
            /*serviceCollection.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
            });*/

            serviceCollection.SwaggerDocument(documentOptions =>
            {
                documentOptions.MaxEndpointVersion = 2;
                documentOptions.DocumentSettings = s =>
                {
                    s.DocumentName = "V2";
                    s.Version = "2.0";
                };
                documentOptions.ShortSchemaNames = true;
            });

            serviceCollection.SwaggerDocument(documentOptions =>
            {
                documentOptions.MaxEndpointVersion = 1;
                documentOptions.DocumentSettings = s =>
                {
                    s.DocumentName = "V1";
                    s.Version = "1.0";
                };

                documentOptions.ShortSchemaNames = true;
            });

            serviceCollection.AddQuartzHostedService();

            serviceCollection.AddHttpContextAccessor();

            serviceCollection.AddHealthChecks();

            return serviceCollection;
        }
    }
}