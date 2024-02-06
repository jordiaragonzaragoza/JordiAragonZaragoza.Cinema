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
        private readonly SqlEdgeContainer writeStoreContainer =
            new SqlEdgeBuilder()
            .WithImage("mcr.microsoft.com/azure-sql-edge:latest")
            .WithName("azuresqledge.cinema.reservation.writestore.functionaltests.presentation.webapi")
            .WithAutoRemove(true).Build();

        private readonly SqlEdgeContainer readStoreContainer =
            new SqlEdgeBuilder()
            .WithImage("mcr.microsoft.com/azure-sql-edge:latest")
            .WithName("azuresqledge.cinema.reservation.readstore.functionaltests.presentation.webapi")
            .WithAutoRemove(true).Build();

        private SqlConnection writeStoreConnection;
        private SqlConnection readStoreConnection;
        private CustomWebApplicationFactory<TProgram> customApplicationFactory;
        private IServiceScopeFactory scopeFactory;
        private Respawner writeStoreRespawner;
        private Respawner readStoreRespawner;
        private bool disposedValue;

        public HttpClient HttpClient { get; private set; }

        public async Task InitializeAsync()
        {
            await this.StartDbsConnectionsAsync();

            this.customApplicationFactory = new CustomWebApplicationFactory<TProgram>(this.writeStoreConnection, this.readStoreConnection);

            this.HttpClient = this.customApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            this.scopeFactory = this.customApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>();

            this.writeStoreRespawner = await Respawner.CreateAsync(this.writeStoreContainer.GetConnectionString(), new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
            });

            this.readStoreRespawner = await Respawner.CreateAsync(this.readStoreContainer.GetConnectionString(), new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
            });
        }

        public void InitDatabases()
        {
            this.InitWriteStoreDatabase();
            this.InitReadStoreDatabase();
        }

        public async Task ResetDatabasesAsync()
        {
            var resetWriteStoreTask = this.writeStoreRespawner.ResetAsync(this.writeStoreContainer.GetConnectionString());
            var resetReadStoreTask = this.readStoreRespawner.ResetAsync(this.readStoreContainer.GetConnectionString());

            await Task.WhenAll(resetWriteStoreTask, resetReadStoreTask);
        }

        public async Task DisposeAsync()
        {
            var disposeWriteConnectionTask = this.writeStoreConnection.DisposeAsync();
            var disposeReadConnectionTask = this.readStoreConnection.DisposeAsync();

            await Task.WhenAll(disposeWriteConnectionTask.AsTask(), disposeReadConnectionTask.AsTask());

            var disposeWriteContainerTask = this.writeStoreContainer.DisposeAsync();
            var disposeReadContainerTask = this.readStoreContainer.DisposeAsync();

            await Task.WhenAll(disposeWriteContainerTask.AsTask(), disposeReadContainerTask.AsTask());
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
            var startWriteContainerTask = this.writeStoreContainer.StartAsync();
            var startReadContainerTask = this.readStoreContainer.StartAsync();

            await Task.WhenAll(startWriteContainerTask, startReadContainerTask);

            this.writeStoreConnection = new SqlConnection(this.writeStoreContainer.GetConnectionString());
            this.readStoreConnection = new SqlConnection(this.readStoreContainer.GetConnectionString());
        }

        private void InitWriteStoreDatabase()
        {
            using var writeScope = this.scopeFactory.CreateScope();
            var writeContext = writeScope.ServiceProvider.GetRequiredService<ReservationWriteContext>();
            var logger = writeScope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

            try
            {
                SeedData.PopulateWriteTestData(writeContext);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred seeding the write database with test data. Error: {exceptionMessage}", exception.Message);
            }
        }

        private void InitReadStoreDatabase()
        {
            using var readScope = this.scopeFactory.CreateScope();
            var readContext = readScope.ServiceProvider.GetRequiredService<ReservationReadContext>();
            var logger = readScope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

            try
            {
                SeedData.PopulateReadTestData(readContext);
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred seeding the read database with test data. Error: {exceptionMessage}", exception.Message);
            }
        }
    }
}