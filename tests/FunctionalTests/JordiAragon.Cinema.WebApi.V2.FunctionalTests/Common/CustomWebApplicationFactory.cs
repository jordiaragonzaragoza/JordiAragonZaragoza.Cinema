namespace JordiAragon.Cinema.WebApi.V2.FunctionalTests.Common
{
    using System.Data.Common;
    using JordiAragon.Cinema.Infrastructure.EntityFramework;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Hosting;

    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        private readonly DbConnection connection;

        public CustomWebApplicationFactory(DbConnection connection)
        {
            this.connection = connection;
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
                    .RemoveAll<DbContextOptions<CinemaContext>>()
                    .AddDbContext<CinemaContext>((options) =>
                    {
                        options.UseSqlServer(this.connection);
                    });
            });
        }
    }
}