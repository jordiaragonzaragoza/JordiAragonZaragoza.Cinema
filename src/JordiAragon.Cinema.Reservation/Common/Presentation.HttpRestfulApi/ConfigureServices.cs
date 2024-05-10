namespace JordiAragon.Cinema.Reservation.Common.Presentation.HttpRestfulApi
{
    using FastEndpoints;
    using FastEndpoints.Swagger;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ConfigureServices
    {
        public static IServiceCollection AddHttpRestfulApiServices(this IServiceCollection serviceCollection, IConfiguration configuration)
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

            serviceCollection.AddFastEndpoints();

            serviceCollection.AddAuthentication();

            serviceCollection.AddAuthorization();

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

            serviceCollection.AddHttpContextAccessor();

            serviceCollection.AddHealthChecks();

            return serviceCollection;
        }
    }
}