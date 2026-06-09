namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Command.Common
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Testcontainers.KurrentDb;
    using Xunit;

    public class FunctionalTestsFixture<TProgram> : IAsyncLifetime, IDisposable
        where TProgram : class
    {
        private readonly KurrentDbContainer eventStoreContainer =
            new KurrentDbBuilder($"{Constants.KurrentDbImage}:{Constants.KurrentDbArmImageTag}")
            .WithName($"kurrentdb.cinema.reservation.eventstore.functionaltests.api.command-{Guid.CreateVersion7():N}")
            .WithAutoRemove(true).Build();

        private string eventStoreConnection = default!;
        private CustomWebApplicationFactory<TProgram> customApplicationFactory = default!;
        private IServiceScopeFactory scopeFactory = default!;
        private bool disposedValue;

        public HttpClient HttpClient { get; private set; } = default!;

        public async Task InitializeAsync()
        {
            await this.StartDbsConnectionAsync();

            this.customApplicationFactory = new CustomWebApplicationFactory<TProgram>(this.eventStoreConnection);

            this.HttpClient = this.customApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            this.scopeFactory = this.customApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>();
        }

        public async Task InitDatabaseAsync(CancellationToken cancellationToken = default)
        {
            await this.InitEventStoreDatabaseAsync(cancellationToken);
        }

        public async Task ResetDatabaseAsync()
        {
            // TODO: Implement reset logic
            ////await this.readModelStoreRespawner.ResetAsync(this.eventStoreConnection);

            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            ////await this.eventStoreConnection.DisposeAsync();
            await this.eventStoreContainer.DisposeAsync();
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
            await this.eventStoreContainer.StartAsync();
            this.eventStoreConnection = this.eventStoreContainer.GetConnectionString();
            ////this.eventStoreConnection = new NpgsqlConnection(this.eventStoreContainer.GetConnectionString());
            ////await this.eventStoreConnection.OpenAsync();
        }

        private async Task InitEventStoreDatabaseAsync(CancellationToken stoppingToken = default)
        {
            using var readModelScope = this.scopeFactory.CreateScope();
            var eventStore = readModelScope.ServiceProvider.GetRequiredService<IEventStore>();
            var logger = readModelScope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

            try
            {
                logger.LogInformation("Starting seeding data on business model");

                await SeedData.PopulateBusinessModelTestDataAsync(eventStore, stoppingToken);

                logger.LogInformation("Data seeding completed successfully");
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "An error occurred seeding the business model database with test data. Error: {ExceptionMessage}", exception.Message);

                throw;
            }
        }
    }
}