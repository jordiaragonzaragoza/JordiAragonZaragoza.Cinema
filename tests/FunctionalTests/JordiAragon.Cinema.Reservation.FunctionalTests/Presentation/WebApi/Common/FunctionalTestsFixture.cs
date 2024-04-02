namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.WebApi.Common
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
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
        private readonly SqlEdgeContainer businessModelStoreContainer =
            new SqlEdgeBuilder()
            .WithImage("mcr.microsoft.com/azure-sql-edge:latest")
            .WithName("azuresqledge.cinema.reservation.businessmodelstore.functionaltests.presentation.webapi")
            .WithAutoRemove(true).Build();

        private readonly SqlEdgeContainer readModelStoreContainer =
            new SqlEdgeBuilder()
            .WithImage("mcr.microsoft.com/azure-sql-edge:latest")
            .WithName("azuresqledge.cinema.reservation.readmodelstore.functionaltests.presentation.webapi")
            .WithAutoRemove(true).Build();

        private SqlConnection businessModelStoreConnection;
        private SqlConnection readModelStoreConnection;
        private CustomWebApplicationFactory<TProgram> customApplicationFactory;
        private IServiceScopeFactory scopeFactory;
        private Respawner businessModelStoreRespawner;
        private Respawner readModelStoreRespawner;
        private bool disposedValue;

        public HttpClient HttpClient { get; private set; }

        public async Task InitializeAsync()
        {
            await this.StartDbsConnectionsAsync();

            this.customApplicationFactory = new CustomWebApplicationFactory<TProgram>(this.businessModelStoreConnection, this.readModelStoreConnection);

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
            this.InitReadModelStoreDatabase();
        }

        public async Task ResetDatabasesAsync()
        {
            var resetBusinessModelStoreTask = this.businessModelStoreRespawner.ResetAsync(this.businessModelStoreContainer.GetConnectionString());
            var resetReadModelStoreTask = this.readModelStoreRespawner.ResetAsync(this.readModelStoreContainer.GetConnectionString());

            await Task.WhenAll(resetBusinessModelStoreTask, resetReadModelStoreTask);
        }

        public async Task DisposeAsync()
        {
            var disposeBusinessModelConnectionTask = this.businessModelStoreConnection.DisposeAsync();
            var disposeReadModelConnectionTask = this.readModelStoreConnection.DisposeAsync();

            await Task.WhenAll(disposeBusinessModelConnectionTask.AsTask(), disposeReadModelConnectionTask.AsTask());

            var disposeBusinessModelContainerTask = this.businessModelStoreContainer.DisposeAsync();
            var disposeReadModelContainerTask = this.readModelStoreContainer.DisposeAsync();

            await Task.WhenAll(disposeBusinessModelContainerTask.AsTask(), disposeReadModelContainerTask.AsTask());
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

        private async Task StartDbsConnectionsAsync()
        {
            var startBusinessModelContainerTask = this.businessModelStoreContainer.StartAsync();
            var startReadModelContainerTask = this.readModelStoreContainer.StartAsync();

            await Task.WhenAll(startBusinessModelContainerTask, startReadModelContainerTask);

            this.businessModelStoreConnection = new SqlConnection(this.businessModelStoreContainer.GetConnectionString());
            this.readModelStoreConnection = new SqlConnection(this.readModelStoreContainer.GetConnectionString());
        }

        private void InitBusinessModelStoreDatabase()
        {
            using var businessModelScope = this.scopeFactory.CreateScope();
            var writeContext = businessModelScope.ServiceProvider.GetRequiredService<ReservationBusinessModelContext>();
            var logger = businessModelScope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

            try
            {
                SeedData.PopulateBusinessModelTestData(writeContext, true);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred seeding the business model database with test data. Error: {exceptionMessage}", exception.Message);
            }
        }

        private void InitReadModelStoreDatabase()
        {
            using var readScope = this.scopeFactory.CreateScope();
            var readContext = readScope.ServiceProvider.GetRequiredService<ReservationReadModelContext>();
            var logger = readScope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

            try
            {
                SeedData.PopulateReadModelTestData(readContext, true);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred seeding the read model database with test data. Error: {exceptionMessage}", exception.Message);
            }
        }
    }
}