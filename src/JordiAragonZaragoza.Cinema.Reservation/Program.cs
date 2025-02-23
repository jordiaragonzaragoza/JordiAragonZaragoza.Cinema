namespace JordiAragonZaragoza.Cinema.Reservation
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using JordiAragonZaragoza.Cinema.ServiceDefaults;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Application;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Migrations;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Presentation.HttpRestfulApi;
    using JordiAragonZaragoza.SharedKernel.Application.AssemblyConfiguration;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.AssemblyConfiguration;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore.AssemblyConfiguration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Constants = JordiAragonZaragoza.Cinema.SharedKernel.Constants;
    using EventStoreModule = JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EventStore.Configuration.EventStoreModule;
    using SharedKernelApplicationModule = JordiAragonZaragoza.SharedKernel.Application.AssemblyConfiguration.ApplicationModule;
    using SharedKernelDomainModule = JordiAragonZaragoza.SharedKernel.Domain.AssemblyConfiguration.DomainModule;
    using SharedKernelEntityFrameworkModule = JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.AssemblyConfiguration.EntityFrameworkModule;
    using SharedKernelEventStoreModule = JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore.AssemblyConfiguration.EventStoreModule;
    using SharedKernelHttpRestfulApiModule = JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.AssemblyConfiguration.HttpRestfulApiModule;
    using SharedKernelInfrastructureModule = JordiAragonZaragoza.SharedKernel.Infrastructure.AssemblyConfiguration.InfrastructureModule;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "Program class should not have a protected constructor or the static keyword because is used in WebApplicationFactory for functional and integration test.")]
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.AddServiceDefaults();
            builder.AddSeqEndpoint(Constants.SeqServer, static settings =>
            {
                settings.DisableHealthChecks = true;
            });

            var configuration = builder.Configuration;

            configuration
                .AddSharedKernelInfrastructureDefaultConfiguration(builder.Environment.EnvironmentName);

            configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            // Add services to the container.
            builder.Services.AddApplicationServices(configuration);
            builder.Services.AddHttpRestfulApiServices(configuration);
            builder.Services.AddSharedKernelInfrastructureServices(configuration);
            builder.Services.AddEntityFrameworkServices(configuration, builder.Environment.EnvironmentName == "Development");
            builder.EnrichDbContexts();

            // TODO: Temporal removed. Will be enabled on using event sourcing as a aggregates store.
            ////builder.Services.AddSharedKernelEventStoreServices(configuration);

            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
            {
                containerBuilder.RegisterModule(new SharedKernelDomainModule());
                containerBuilder.RegisterModule(new SharedKernelApplicationModule());
                containerBuilder.RegisterModule(new SharedKernelInfrastructureModule());
                containerBuilder.RegisterModule(new SharedKernelEntityFrameworkModule());
                containerBuilder.RegisterModule(new EntityFrameworkModule());
                containerBuilder.RegisterModule(new SharedKernelEventStoreModule());
                containerBuilder.RegisterModule(new EventStoreModule());
                containerBuilder.RegisterModule(new SharedKernelHttpRestfulApiModule());
            });

            builder.Host.AddHostBuilderConfigurations();
            builder.WebHost.AddWebHostBuilderConfigurations();

            var app = builder.Build();

            app.Logger.LogInformation("App created...");

            ConfigureWebApplication.AddWebApplicationConfigurations(app);

            MigrationsApplier.Initialize(app, builder.Environment.EnvironmentName == "Development");
            ////SeedData.Initialize(app, builder.Environment.EnvironmentName == "Development");

            app.Run();
        }
    }
}