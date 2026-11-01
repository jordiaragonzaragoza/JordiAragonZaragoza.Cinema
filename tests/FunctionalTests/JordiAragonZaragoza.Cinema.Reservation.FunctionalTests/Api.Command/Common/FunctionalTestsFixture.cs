namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Command.Common
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;

    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.EntityFrameworkCore;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Npgsql;
    using Respawn;

    using Testcontainers.KurrentDb;
    using Testcontainers.PostgreSql;
    using Xunit;

    using ExecutionContext = JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces.ExecutionContext;

    public class FunctionalTestsFixture<TProgram> : IAsyncLifetime, IDisposable
        where TProgram : class
    {
        private readonly KurrentDbContainer eventStoreContainer =
            new KurrentDbBuilder($"{Constants.KurrentDbImage}:{Constants.KurrentDbArmImageTag}")
            .WithName($"kurrentdb.cinema.reservation.eventstore.functionaltests.api.command-{Guid.CreateVersion7():N}")
            .WithAutoRemove(true).Build();

        private readonly PostgreSqlContainer readModelStoreContainer =
            new PostgreSqlBuilder($"{Constants.PostgresImage}:{Constants.PostgresImageTag}")
            .WithName($"postgres.cinema.reservation.readmodelstore.functionaltests.api.command-{Guid.CreateVersion7():N}")
            .WithAutoRemove(true).Build();

        private string eventStoreConnection = default!;
        private NpgsqlConnection readModelStoreConnection = default!;
        private CustomWebApplicationFactory<TProgram> customApplicationFactory = default!;
        private IServiceScopeFactory scopeFactory = default!;
        private Respawner readModelStoreRespawner = default!;
        private bool disposedValue;

        public HttpClient HttpClient { get; private set; } = default!;

        public async Task InitializeAsync()
        {
            await this.StartDbsConnectionAsync();

            this.customApplicationFactory = new CustomWebApplicationFactory<TProgram>(
                this.eventStoreConnection,
                this.readModelStoreConnection);

            this.HttpClient = this.CreateClient(authenticated: true);

            this.scopeFactory = this.customApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>();
        }

        public async Task InitDatabaseAsync(CancellationToken cancellationToken = default)
        {
            await this.InitReadModelStoreDatabaseAsync();
            await this.InitEventStoreDatabaseAsync(cancellationToken);
        }

        public async Task ResetDatabaseAsync()
        {
            // TODO: Implement reset logic for event store if needed. Currently, only the read model store is reset.
            await this.readModelStoreRespawner.ResetAsync(this.readModelStoreConnection);
        }

        public async Task DisposeAsync()
        {
            await this.readModelStoreConnection.DisposeAsync();
            await this.readModelStoreContainer.DisposeAsync();
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

        private static ExecutionContext CreateSeederExecutionContext()
        {
            var testContext = new ExecutionContext(
                actorId: ExecutionContext.CreateServiceActorId("functional-tests"),
                actorType: ActorType.System,
                executor: nameof(FunctionalTestsFixture<TProgram>),
                executorType: ExecutorType.Tool,
                correlationId: Guid.CreateVersion7(),
                causationId: null,
                scopeContext: new ScopeContext(
                    tenantId: SeedData.ExampleTenant.Id,
                    partitionId: SeedData.ExamplePartition.Id,
                    domainId: SeedData.ExampleCinema.Id));

            return testContext;
        }

        private async Task StartDbsConnectionAsync()
        {
            await Task.WhenAll(
                this.eventStoreContainer.StartAsync(),
                this.readModelStoreContainer.StartAsync());

            this.eventStoreConnection = this.eventStoreContainer.GetConnectionString();

            this.readModelStoreConnection = new NpgsqlConnection(this.readModelStoreContainer.GetConnectionString());
            await this.readModelStoreConnection.OpenAsync();
        }

        private async Task InitEventStoreDatabaseAsync(CancellationToken stoppingToken = default)
        {
            using var scope = this.scopeFactory.CreateScope();
            var eventStore = scope.ServiceProvider.GetRequiredService<IEventStore>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();
            var executionContextService = scope.ServiceProvider.GetRequiredService<IExecutionContextService>();
            executionContextService.SetExecutionContext(CreateSeederExecutionContext());
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
            finally
            {
                executionContextService.ClearExecutionContext();
            }
        }

        private async Task InitReadModelStoreDatabaseAsync()
        {
            using var readModelScope = this.scopeFactory.CreateScope();
            var readContext = readModelScope.ServiceProvider.GetRequiredService<ReservationReadModelContext>();
            var logger = readModelScope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

            try
            {
                await readContext.Database.MigrateAsync();

                SeedData.PopulateReadModelTestData(readContext);

                this.readModelStoreRespawner = await Respawner.CreateAsync(
                    this.readModelStoreConnection,
                    new RespawnerOptions
                    {
                        DbAdapter = DbAdapter.Postgres,
                        TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
                    });
            }
            catch (Exception exception)
            {
                logger.LogError(
                    exception,
                    "An error occurred seeding the read model database with test data. Error: {ExceptionMessage}",
                    exception.Message);
                throw;
            }
        }

        private HttpClient CreateClient(bool authenticated = true)
        {
            var client = this.customApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            client.DefaultRequestHeaders.Add("x-tenant-id", SeedData.ExampleTenant.Id.ToString());
            client.DefaultRequestHeaders.Add("x-partition-id", SeedData.ExamplePartition.Id.ToString());
            client.DefaultRequestHeaders.Add("x-domain-id", SeedData.ExampleCinema.Id.ToString());

            if (authenticated)
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(AuthHandler.SchemeName, SeedData.ExampleUserWithAdminRole.Id.ToString());
            }

            return client;
        }
    }
}