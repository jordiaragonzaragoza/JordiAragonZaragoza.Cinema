namespace JordiAragon.Cinema.Reservation
{
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using JordiAragon.Cinema.Reservation.Common.Application;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EventStore.Configuration;
    using JordiAragon.Cinema.Reservation.Common.Presentation.WebApi;
    using JordiAragon.SharedKernel.Application.AssemblyConfiguration;
    using JordiAragon.SharedKernel.Infrastructure.AssemblyConfiguration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using SharedKernelApplicationModule = JordiAragon.SharedKernel.Application.AssemblyConfiguration.ApplicationModule;
    using SharedKernelDomainModule = JordiAragon.SharedKernel.Domain.AssemblyConfiguration.DomainModule;
    using SharedKernelEntityFrameworkModule = JordiAragon.SharedKernel.Infrastructure.EntityFramework.AssemblyConfiguration.EntityFrameworkModule;
    using SharedKernelEventStoreModule = JordiAragon.SharedKernel.Infrastructure.EventStore.AssemblyConfiguration.EventStoreModule;
    using SharedKernelInfrastructureModule = JordiAragon.SharedKernel.Infrastructure.AssemblyConfiguration.InfrastructureModule;
    using SharedKernelWebApiModule = JordiAragon.SharedKernel.Presentation.WebApi.AssemblyConfiguration.WebApiModule;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1118:Utility classes should not have public constructors", Justification = "Program class should not have a protected constructor or the static keyword because is used in WebApplicationFactory for functional and integration test.")]
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            configuration
                .AddSharedKernelInfrastructureDefaultConfiguration(builder.Environment.EnvironmentName);

            configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            // Add services to the container.
            builder.Services.AddApplicationServices(configuration);
            builder.Services.AddSharedKernelApplicationServices();
            builder.Services.AddWebApiServices(configuration);
            builder.Services.AddSharedKernelInfrastructureServices(configuration, builder.Environment.EnvironmentName == "Development");
            builder.Services.AddEntityFrameworkServices(configuration, builder.Environment.EnvironmentName == "Development");
            builder.Services.AddEventStoreServices(configuration, builder.Environment.ApplicationName);

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
                containerBuilder.RegisterModule(new SharedKernelWebApiModule());
            });

            builder.Host.AddWebApiHostBuilderConfigurations();

            var app = builder.Build();

            app.Logger.LogInformation("App created...");

            ConfigureWebApplication.AddWebApplicationConfigurations(app);

            SeedData.Initialize(app, builder.Environment.EnvironmentName == "Development");

            app.Run();
        }
    }
}