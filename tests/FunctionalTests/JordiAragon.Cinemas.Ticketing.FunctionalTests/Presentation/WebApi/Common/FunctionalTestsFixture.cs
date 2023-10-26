namespace JordiAragon.Cinemas.Ticketing.FunctionalTests.Presentation.WebApi.Common
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using JordiAragon.Cinemas.Ticketing.Common.Infrastructure.EntityFramework;
    using JordiAragon.Cinemas.Ticketing.Common.Infrastructure.EntityFramework.Configuration;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Respawn;
    using Testcontainers.SqlEdge;
    using Xunit;

    public class FunctionalTestsFixture<TProgram> : IAsyncLifetime, IDisposable
        where TProgram : class
    {
        private readonly SqlEdgeContainer container =
            new SqlEdgeBuilder()
            .WithImage("mcr.microsoft.com/azure-sql-edge:latest")
            .WithName("azuresqledge.cinema.ticketing.functionaltests.presentation.webapi")
            .WithAutoRemove(true).Build();

        private SqlConnection connection;
        private CustomWebApplicationFactory<TProgram> customApplicationFactory;
        private IServiceScopeFactory scopeFactory;
        private Respawner respawner;
        private bool disposedValue;

        public HttpClient HttpClient { get; private set; }

        public async Task InitializeAsync()
        {
            await this.container.StartAsync();

            this.connection = new SqlConnection(this.container.GetConnectionString());

            this.customApplicationFactory = new CustomWebApplicationFactory<TProgram>(this.connection);

            this.HttpClient = this.customApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            this.scopeFactory = this.customApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>();

            this.respawner = await Respawner.CreateAsync(this.container.GetConnectionString(), new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
            });
        }

        public void InitDatabase()
        {
            using var scope = this.scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TicketingContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

            try
            {
                SeedData.PopulateTestData(context);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred seeding the database with test data. Error: {exceptionMessage}", exception.Message);
            }
        }

        public async Task ResetDatabaseAsync()
        {
            await this.respawner.ResetAsync(this.container.GetConnectionString());
        }

        public async Task DisposeAsync()
        {
            await this.connection.DisposeAsync();
            await this.container.DisposeAsync();
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.customApplicationFactory.Dispose();
                }

                this.customApplicationFactory = null;
                this.disposedValue = true;
            }
        }
    }
}