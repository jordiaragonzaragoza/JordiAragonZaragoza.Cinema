namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Data.SqlClient;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Respawn;
    using Testcontainers.EventStoreDb;
    using Testcontainers.SqlEdge;
    using Xunit;

    public class FunctionalTestsFixture<TProgram> : IAsyncLifetime, IDisposable
        where TProgram : class
    {
        private readonly SqlEdgeContainer businessModelStoreContainer =
            new SqlEdgeBuilder()
            .WithImage("mcr.microsoft.com/azure-sql-edge:latest")
            .WithName("azuresqledge.cinema.reservation.businessmodelstore.functionaltests.presentation.httprestfulapi")
            .WithAutoRemove(true).Build();

        private readonly EventStoreDbContainer eventStoreContainer =
            new EventStoreDbBuilder()
            .WithImage("eventstore/eventstore:23.10.1-alpha-arm64v8")
            .WithName("eventstoredb.cinema.reservation.eventstore.functionaltests.presentation.httprestfulapi")
            .WithAutoRemove(true).Build();

        private readonly SqlEdgeContainer readModelStoreContainer =
            new SqlEdgeBuilder()
            .WithImage("mcr.microsoft.com/azure-sql-edge:latest")
            .WithName("azuresqledge.cinema.reservation.readmodelstore.functionaltests.presentation.httprestfulapi")
            .WithAutoRemove(true).Build();

        private SqlConnection businessModelStoreConnection;
        private SqlConnection readModelStoreConnection;
        private string eventStoreDbConnectionString;
        private CustomWebApplicationFactory<TProgram> customApplicationFactory;
        private IServiceScopeFactory scopeFactory;
        private Respawner businessModelStoreRespawner;
        private Respawner readModelStoreRespawner;
        private bool disposedValue;

        public HttpClient HttpClient { get; private set; }

        public async Task InitializeAsync()
        {
            await this.StartDbsConnectionsAsync();

            this.customApplicationFactory = new CustomWebApplicationFactory<TProgram>(this.businessModelStoreConnection, this.readModelStoreConnection, this.eventStoreDbConnectionString);

            this.HttpClient = this.customApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            this.scopeFactory = this.customApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>();

            this.businessModelStoreRespawner = await Respawner.CreateAsync(this.businessModelStoreContainer.GetConnectionString(), new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
            });

            this.readModelStoreRespawner = await Respawner.CreateAsync(this.readModelStoreContainer.GetConnectionString(), new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
            });
        }

        public void InitDatabases()
        {
            this.InitBusinessModelStoreDatabase();
        }

        public async Task ResetDatabasesAsync()
        {
            var resetBusinessModelStoreTask = this.businessModelStoreRespawner.ResetAsync(this.businessModelStoreContainer.GetConnectionString());
            var resetReadModelStoreTask = this.readModelStoreRespawner.ResetAsync(this.readModelStoreContainer.GetConnectionString());

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

                this.customApplicationFactory = null;
                this.disposedValue = true;
            }
        }

        private async Task StartDbsConnectionsAsync()
        {
            var startBusinessModelContainerTask = this.businessModelStoreContainer.StartAsync();
            var startReadModelContainerTask = this.readModelStoreContainer.StartAsync();
            var startEventStoreContainerTask = this.eventStoreContainer.StartAsync();

            await Task.WhenAll(startBusinessModelContainerTask, startReadModelContainerTask, startEventStoreContainerTask);

            this.businessModelStoreConnection = new SqlConnection(this.businessModelStoreContainer.GetConnectionString());
            this.readModelStoreConnection = new SqlConnection(this.readModelStoreContainer.GetConnectionString());
            this.eventStoreDbConnectionString = this.eventStoreContainer.GetConnectionString();
        }

        private void InitBusinessModelStoreDatabase()
        {
            using var businessModelScope = this.scopeFactory.CreateScope();
            var writeContext = businessModelScope.ServiceProvider.GetRequiredService<ReservationBusinessModelContext>();
            var logger = businessModelScope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

            try
            {
                SeedData.PopulateBusinessModelTestData(writeContext);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred seeding the business model database with test data. Error: {exceptionMessage}", exception.Message);
            }
        }
    }
}