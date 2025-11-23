namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query
{
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Configuration;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Application;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections.Migrations;
    using JordiAragonZaragoza.SharedKernel.Application;
    using JordiAragonZaragoza.SharedKernel.Infrastructure;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Logging;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "Program class should not have a protected constructor or the static keyword because is used in WebApplicationFactory for functional and integration test.")]
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            builder.AddInfrastructure();

            // Configure specific Host Services (DI)
            builder.Services
                .AddApplication()
                .AddApplicationQueryHandlers()
                .AddInfrastructureEntityFrameworkProjections(configuration, builder.Environment.EnvironmentName == "Development")
                .AddInfrastructure()
                .AddPresentationHttpRestfulApi(configuration);

            // Then configure SharedKernel Services (DI)
            builder.Services
                .AddSharedKernelApplicationQueryBus()
                .AddSharedKernelInfrastructure()
                .AddSharedKernelInfrastructureQueryBus()
                .AddSharedKernelPresentationHttpRestfulApi();

            builder.AddInfrastructureEntityFrameworkProjections();

            builder.Host.UseHostBuilderConfigurations();
            builder.WebHost.UseWebHostBuilderConfigurations();

            var app = builder.Build();

            app.Logger.LogDebug("Api Query Host created...");

            // Configure Request Pipeline
            ConfigureWebApplication.UseWebApplicationConfigurations(app);

            // TODO: Temporal. Apply migrations and seed data only in development environment until we have a proper aspire implementation.
            MigrationsApplier.Initialize(app, builder.Environment.EnvironmentName == "Development");
            ////SeedData.Initialize(app, builder.Environment.EnvironmentName == "Development");

            app.Run();
        }
    }
}