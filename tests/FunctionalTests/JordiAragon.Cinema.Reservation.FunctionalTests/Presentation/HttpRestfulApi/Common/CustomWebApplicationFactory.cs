namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common
{
    using System.Collections.Generic;
    using System.Data.Common;
    using Ardalis.GuardClauses;
    using EventStore.Client;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework;
    using JordiAragon.SharedKernel.Infrastructure.EventStore;
    using JordiAragon.SharedKernel.Infrastructure.EventStore.EventStoreDb;
    using JordiAragon.SharedKernel.Infrastructure.EventStore.EventStoreDb.Subscriptions;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;
    using JordiAragon.SharedKernel.Infrastructure.EventStore.AssemblyConfiguration;
    using Quartz;

    public sealed class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        private readonly DbConnection businessModelStoreConnection;
        private readonly DbConnection readModelStoreConnection;
        private readonly string eventStoreDbConnectionString;

        public CustomWebApplicationFactory(
            DbConnection businessModelStoreConnection,
            DbConnection readModelStoreConnection,
            string eventStoreDbConnectionString)
        {
            this.businessModelStoreConnection = businessModelStoreConnection;
            this.readModelStoreConnection = readModelStoreConnection;
            this.eventStoreDbConnectionString = eventStoreDbConnectionString;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            Guard.Against.Null(builder, nameof(builder));

            builder.UseEnvironment("Development");
            var host = builder.Build();
            host.Start();

            return host;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services
                    .RemoveAll<DbContextOptions<ReservationBusinessModelContext>>()
                    .AddDbContext<ReservationBusinessModelContext>((options) =>
                    {
                        options.UseNpgsql(this.businessModelStoreConnection);
                    });

                services
                    .RemoveAll<DbContextOptions<ReservationReadModelContext>>()
                    .AddDbContext<ReservationReadModelContext>((options) =>
                    {
                        options.UseNpgsql(this.readModelStoreConnection);
                        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    });

                services
                    .Configure<QuartzOptions>(options =>
                    {
                        // TODO: Review. Quartz Clustering is disabled on Functional Test due to an issue with db connection on testcontainers.
                        options.Clear();
                        options["quartz.scheduler.jobFactory.type"] = "Quartz.Simpl.MicrosoftDependencyInjectionJobFactory, Quartz.Extensions.DependencyInjection";
                    });

                services
                    .Configure<EventStoreDbOptions>(options =>
                    {
                        options.ConnectionString = this.eventStoreDbConnectionString;
                    });
            });
        }
    }
}