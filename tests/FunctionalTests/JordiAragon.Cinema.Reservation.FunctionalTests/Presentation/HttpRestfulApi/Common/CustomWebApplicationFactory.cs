namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common
{
    using System.Data.Common;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;
    using Quartz;

    public sealed class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        private readonly DbConnection businessModelStoreConnection;
        private readonly DbConnection readModelStoreConnection;

        public CustomWebApplicationFactory(
            DbConnection businessModelStoreConnection,
            DbConnection readModelStoreConnection)
        {
            this.businessModelStoreConnection = businessModelStoreConnection;
            this.readModelStoreConnection = readModelStoreConnection;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
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
                        options.UseSqlServer(this.businessModelStoreConnection);
                    });

                services
                    .RemoveAll<DbContextOptions<ReservationReadModelContext>>()
                    .AddDbContext<ReservationReadModelContext>((options) =>
                    {
                        options.UseSqlServer(this.readModelStoreConnection);
                        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    });

                services
                    .Configure<QuartzOptions>(options =>
                    {
                        // TODO: Review. Quartz Clustering is disabled on Functional Test due to an issue with db connection on testcontainers.
                        options.Clear();
                        options["quartz.scheduler.jobFactory.type"] = "Quartz.Simpl.MicrosoftDependencyInjectionJobFactory, Quartz.Extensions.DependencyInjection";
                    });
            });
        }
    }
}