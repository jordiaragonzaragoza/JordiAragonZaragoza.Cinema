namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Npgsql;
    using Respawn;
    using Testcontainers.EventStoreDb;
    using Testcontainers.PostgreSql;
    using Xunit;

    public class FunctionalTestsFixture<TProgram> : IAsyncLifetime, IDisposable
        where TProgram : class
    {
        private readonly PostgreSqlContainer businessModelStoreContainer =
            new PostgreSqlBuilder()
            .WithImage("postgres:15.1-alpine")
            .WithName("postgres.cinema.reservation.businessmodelstore.functionaltests.presentation.httprestfulapi")
            .WithAutoRemove(true).Build();

        private readonly EventStoreDbContainer eventStoreContainer =
            new EventStoreDbBuilder()
            .WithImage("eventstore/eventstore:23.10.1-alpha-arm64v8")
            .WithName("eventstoredb.cinema.reservation.eventstore.functionaltests.presentation.httprestfulapi")
            .WithAutoRemove(true).Build();

        private readonly PostgreSqlContainer readModelStoreContainer =
            new PostgreSqlBuilder()
            .WithImage("postgres:15.1-alpine")
            .WithName("postgres.cinema.reservation.readmodelstore.functionaltests.presentation.httprestfulapi")
            .WithAutoRemove(true).Build();

        private NpgsqlConnection businessModelStoreConnection = default!;
        private NpgsqlConnection readModelStoreConnection = default!;
        private string eventStoreDbConnectionString = default!;
        private CustomWebApplicationFactory<TProgram> customApplicationFactory = default!;
        private IServiceScopeFactory scopeFactory = default!;
        private Respawner businessModelStoreRespawner = default!;
        private Respawner readModelStoreRespawner = default!;
        private bool disposedValue;

        public HttpClient HttpClient { get; private set; } = default!;

        public async Task InitializeAsync()
        {
            await this.StartDbsConnectionsAsync();

            this.customApplicationFactory = new CustomWebApplicationFactory<TProgram>(this.businessModelStoreConnection, this.readModelStoreConnection, this.eventStoreDbConnectionString);

            this.HttpClient = this.customApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            this.scopeFactory = this.customApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>();
        }

        public async Task InitDatabasesAsync()
        {
            var initBusinessDatabaseTask = this.InitBusinessModelStoreDatabaseAsync();
            var initReadModelDatabaseTask = this.InitReadModelStoreDatabaseAsync();

            await Task.WhenAll(initBusinessDatabaseTask, initReadModelDatabaseTask);
        }

        public async Task ResetDatabasesAsync()
        {
            var resetBusinessModelStoreTask = this.businessModelStoreRespawner.ResetAsync(this.businessModelStoreConnection);
            var resetReadModelStoreTask = this.readModelStoreRespawner.ResetAsync(this.readModelStoreConnection);

            await Task.WhenAll(resetBusinessModelStoreTask, resetReadModelStoreTask);
        }

        public async Task DisposeAsync()
        {
            ////await Task.Delay(2000);

            var disposeBusinessModelConnectionTask = this.businessModelStoreConnection.DisposeAsync();
            var disposeReadModelConnectionTask = this.readModelStoreConnection.DisposeAsync();

            await Task.WhenAll(disposeBusinessModelConnectionTask.AsTask(), disposeReadModelConnectionTask.AsTask());

            var disposeBusinessModelContainerTask = this.businessModelStoreContainer.DisposeAsync();
            var disposeReadModelContainerTask = this.readModelStoreContainer.DisposeAsync();
            var disposeEventStoreContainerTask = this.eventStoreContainer.DisposeAsync();

            await Task.WhenAll(disposeBusinessModelContainerTask.AsTask(), disposeReadModelContainerTask.AsTask(), disposeEventStoreContainerTask.AsTask());
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

        private async Task StartDbsConnectionsAsync()
        {
            var startBusinessModelContainerTask = this.businessModelStoreContainer.StartAsync();
            var startReadModelContainerTask = this.readModelStoreContainer.StartAsync();
            var startEventStoreContainerTask = this.eventStoreContainer.StartAsync();

            await Task.WhenAll(startBusinessModelContainerTask, startReadModelContainerTask, startEventStoreContainerTask);

            this.businessModelStoreConnection = new NpgsqlConnection(this.businessModelStoreContainer.GetConnectionString());
            await this.businessModelStoreConnection.OpenAsync();

            this.readModelStoreConnection = new NpgsqlConnection(this.readModelStoreContainer.GetConnectionString());
            await this.readModelStoreConnection.OpenAsync();

            this.eventStoreDbConnectionString = this.eventStoreContainer.GetConnectionString();
        }

        private async Task InitBusinessModelStoreDatabaseAsync()
        {
            using var businessModelScope = this.scopeFactory.CreateScope();
            var writeContext = businessModelScope.ServiceProvider.GetRequiredService<ReservationBusinessModelContext>();
            var logger = businessModelScope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

            try
            {
                SeedData.PopulateBusinessModelTestData(writeContext);

                this.businessModelStoreRespawner = await Respawner.CreateAsync(this.businessModelStoreConnection, new RespawnerOptions
                {
                    DbAdapter = DbAdapter.Postgres,
                    TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
                });
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred seeding the business model database with test data. Error: {exceptionMessage}", exception.Message);
            }
        }

        private async Task InitReadModelStoreDatabaseAsync()
        {
            this.readModelStoreRespawner = await Respawner.CreateAsync(this.readModelStoreConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
            });
        }
    }
}