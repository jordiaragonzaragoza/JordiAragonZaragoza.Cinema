namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command
{
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Configuration;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Application;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore.Business;
    using JordiAragonZaragoza.SharedKernel.Application;
    using JordiAragonZaragoza.SharedKernel.Domain;
    using JordiAragonZaragoza.SharedKernel.Infrastructure;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore.AssemblyConfiguration;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Logging;

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Major Code Smell",
        "S1118:Utility classes should not have public constructors",
        Justification = "Used by WebApplicationFactory for functional and integration tests.")]
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            builder.AddInfrastructure();
            builder.AddInfrastructureEventStoreDbClient();

            // Configure specific Host Services (DI)
            builder.Services
                .AddDomain()
                .AddApplicationValidators()
                .AddApplicationCommandHandlers()
                .AddInfrastructureEventStoreDbRepositories()
                .AddPresentationHttpRestfulApi(configuration);

            // Then configure SharedKernel Services (DI)
            builder.Services
                .AddSharedKernelDomain()
                .AddSharedKernelApplicationCommandBus()
                .AddSharedKernelInfrastructureEventStoreDbBusiness(configuration)
                .AddSharedKernelInfrastructure()
                .AddSharedKernelInfrastructureCommandBus()
                .AddSharedKernelPresentationHttpRestfulApi();

            builder.Host.UseHostBuilderConfigurations();
            builder.WebHost.UseWebHostBuilderConfigurations();

            var app = builder.Build();

            app.Logger.LogDebug("Reservation Api Command Host created...");

            // Configure Request Pipeline
            ConfigureWebApplication.UseWebApplicationConfigurations(app);

            app.Run();
        }
    }
}