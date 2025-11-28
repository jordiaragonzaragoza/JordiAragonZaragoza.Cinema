namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Command.Common
{
    using System;
    using System.Data.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;

    public sealed class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        private readonly DbConnection readModelStoreConnection;

        public CustomWebApplicationFactory(
            DbConnection readModelStoreConnection)
        {
            this.readModelStoreConnection = readModelStoreConnection;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

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
                    .RemoveAll<DbContextOptions<ReservationReadModelContext>>()
                    .AddDbContext<ReservationReadModelContext>((options) =>
                    {
                        options.UseNpgsql(this.readModelStoreConnection);
                        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    });
            });
        }
    }
}