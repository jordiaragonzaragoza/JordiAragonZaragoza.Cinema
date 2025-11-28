namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Command.Common
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Npgsql;
    using Respawn;
    using Testcontainers.KurrentDb;

    using Testcontainers.PostgreSql;
    using Xunit;

    public class FunctionalTestsFixture<TProgram> : IAsyncLifetime, IDisposable
        where TProgram : class
    {
        private readonly KurrentDbContainer businessModelStoreContainer =
            new KurrentDbBuilder()
            .WithImage("25.1.0-experimental-arm64-8.0-jammy")
            .WithName("kurrentdb.cinema.reservation.eventstore.functionaltests.api.command")
            .WithAutoRemove(true).Build();

        ////private NpgsqlConnection readModelStoreConnection = default!;
        private CustomWebApplicationFactory<TProgram> customApplicationFactory = default!;        private bool disposedValue;

        public HttpClient HttpClient { get; private set; } = default!;

        public async Task InitializeAsync()
        {
            await this.StartDbsConnectionAsync();

            this.customApplicationFactory = new CustomWebApplicationFactory<TProgram>(this.readModelStoreConnection);

            this.HttpClient = this.customApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            this.scopeFactory = this.customApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>();
        }

        public async Task InitDatabaseAsync()
        {
            await this.InitReadModelStoreDatabaseAsync();
        }

        public async Task ResetDatabaseAsync()
        {
            await this.readModelStoreRespawner.ResetAsync(this.readModelStoreConnection);
        }

        public async Task DisposeAsync()
        {
            await this.readModelStoreConnection.DisposeAsync();
            await this.businessModelStoreContainer.DisposeAsync();
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
                    this.customApplicationFactory?.Dispose();
                }

                this.customApplicationFactory = null!;
                this.disposedValue = true;
            }
        }

        private async Task StartDbsConnectionAsync()
        {
            await this.businessModelStoreContainer.StartAsync();

            this.readModelStoreConnection = new NpgsqlConnection(this.businessModelStoreContainer.GetConnectionString());
            await this.readModelStoreConnection.OpenAsync();
        }

        /*private async Task InitReadModelStoreDatabaseAsync()
        {
            using var readModelScope = this.scopeFactory.CreateScope();
            var readContext = readModelScope.ServiceProvider.GetRequiredService<ReservationReadModelContext>();
            var logger = readModelScope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

            try
            {
                SeedData.PopulateReadModelTestData(readContext);

                this.readModelStoreRespawner = await Respawner.CreateAsync(this.readModelStoreConnection, new RespawnerOptions
                {
                    DbAdapter = DbAdapter.Postgres,
                    TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
                });
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred seeding the read model database with test data. Error: {ExceptionMessage}", exception.Message);

                throw;
            }
        }*/
    }
}