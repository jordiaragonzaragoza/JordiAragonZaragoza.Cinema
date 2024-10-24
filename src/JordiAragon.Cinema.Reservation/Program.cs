namespace JordiAragon.Cinema.Reservation
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using JordiAragon.Cinema.ServiceDefaults;
    using JordiAragon.Cinema.Reservation.Common.Application;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Migrations;
    using JordiAragon.Cinema.Reservation.Common.Presentation.HttpRestfulApi;
    using JordiAragon.SharedKernel.Application.AssemblyConfiguration;
    using JordiAragon.SharedKernel.Infrastructure.AssemblyConfiguration;
    using JordiAragon.SharedKernel.Infrastructure.EventStore.AssemblyConfiguration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Constants = JordiAragon.Cinema.SharedKernel.Constants;
    using EventStoreModule = JordiAragon.Cinema.Reservation.Common.Infrastructure.EventStore.Configuration.EventStoreModule;
    using SharedKernelApplicationModule = JordiAragon.SharedKernel.Application.AssemblyConfiguration.ApplicationModule;
    using SharedKernelDomainModule = JordiAragon.SharedKernel.Domain.AssemblyConfiguration.DomainModule;
    using SharedKernelEntityFrameworkModule = JordiAragon.SharedKernel.Infrastructure.EntityFramework.AssemblyConfiguration.EntityFrameworkModule;
    using SharedKernelEventStoreModule = JordiAragon.SharedKernel.Infrastructure.EventStore.AssemblyConfiguration.EventStoreModule;
    using SharedKernelHttpRestfulApiModule = JordiAragon.SharedKernel.Presentation.HttpRestfulApi.AssemblyConfiguration.HttpRestfulApiModule;
    using SharedKernelInfrastructureModule = JordiAragon.SharedKernel.Infrastructure.AssemblyConfiguration.InfrastructureModule;

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
            builder.Services.AddSharedKernelInfrastructureServices(configuration, builder.Environment.EnvironmentName == "Development");
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