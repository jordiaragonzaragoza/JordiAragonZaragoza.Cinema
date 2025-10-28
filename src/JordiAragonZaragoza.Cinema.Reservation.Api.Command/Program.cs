namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command
{
    using JordiAragonZaragoza.SharedKernel.Application;
    using JordiAragonZaragoza.SharedKernel.Domain;
    using JordiAragonZaragoza.SharedKernel.Infrastructure;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Configuration;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Application;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore.AssemblyConfiguration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Logging;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore.Business;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "Program class should not have a protected constructor or the static keyword because is used in WebApplicationFactory for functional and integration test.")]
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Configure specific Host Services (DI)
            builder.Services
                .AddDomain()
                .AddApplication()
                .AddInfrastructureEventStoreDbBusiness()
                .AddInfrastructure()
                .AddPresentationHttpRestfulApi(configuration);

            // Then configure SharedKernel Services (DI)
            builder.Services
                .AddSharedKernelDomain()
                .AddSharedKernelApplication()
                .AddSharedKernelInfrastructureEventStoreDbBusiness(configuration)
                .AddSharedKernelInfrastructure(
                            AssemblyReference.Assembly)
                .AddSharedKernelPresentationHttpRestfulApi();

            builder.Host.UseHostBuilderConfigurations();
            builder.WebHost.UseWebHostBuilderConfigurations();

            var app = builder.Build();

            app.Logger.LogDebug("Api Command Host created...");

            // Configure Request Pipeline
            ConfigureWebApplication.UseWebApplicationConfigurations(app);

            app.Run();
        }
    }
}