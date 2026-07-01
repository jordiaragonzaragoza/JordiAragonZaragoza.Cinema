namespace JordiAragonZaragoza.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common
{
    using System;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Interceptors;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Npgsql;
    using NSubstitute;
    using Respawn;
    using Testcontainers.PostgreSql;
    using Xunit;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Temporal suppresion")]
    public sealed class IntegrationTestsFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer readModelStoreContainer =
            new PostgreSqlBuilder($"{Constants.PostgresImage}:{Constants.PostgresImageTag}")
            .WithName($"postgres.cinema.reservation.readmodelstore.integrationtests.infrastructure.entityframework-{Guid.CreateVersion7():N}")
            .WithAutoRemove(true).Build();

        private NpgsqlConnection readModelStoreConnection = default!;
        private Respawner readModelStoreRespawner = default!;

        public string ConnectionString => this.readModelStoreConnection.ConnectionString;

        public async Task InitializeAsync()
        {
            await this.InitializeReadModelStoreConnectionAsync();
        }

        public async Task EnsureDatabaseInitializedAsync()
        {
            var options = this.CreateReadModelContextOptions();
            var mockLoggerFactory = Substitute.For<ILoggerFactory>();
            var mockHostEnvironment = Substitute.For<IHostEnvironment>();
            var nullInterceptor = CreateNullExecutionContextService();

            await using var migrationContext = new ReservationReadModelContext(
                options,
                mockLoggerFactory,
                mockHostEnvironment,
                new TenantReadModelSaveChangesInterceptor(nullInterceptor));

            await migrationContext.Database.MigrateAsync();

            this.readModelStoreRespawner = await Respawner.CreateAsync(
                this.readModelStoreConnection,
                new RespawnerOptions
                {
                    DbAdapter = DbAdapter.Postgres,
                    TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
                });
        }

        public ReservationReadModelContext CreateReadModelContext(
            IExecutionContextService executionContextService)
        {
            var options = this.CreateReadModelContextOptions();
            var mockLoggerFactory = Substitute.For<ILoggerFactory>();
            var mockHostEnvironment = Substitute.For<IHostEnvironment>();
            var interceptor = new TenantReadModelSaveChangesInterceptor(executionContextService);

            return new ReservationReadModelContext(
                options, mockLoggerFactory, mockHostEnvironment, interceptor);
        }

        public async Task ResetDatabasesAsync()
            => await this.readModelStoreRespawner.ResetAsync(this.readModelStoreConnection);

        public async Task DisposeAsync()
        {
            await this.readModelStoreConnection.DisposeAsync();
            await this.readModelStoreContainer.DisposeAsync();
        }

        public DbContextOptions<ReservationReadModelContext> CreateReadModelContextOptions()
        {
            var builder = new DbContextOptionsBuilder<ReservationReadModelContext>();
            builder.UseNpgsql(this.readModelStoreConnection);
            return builder.Options;
        }

        private static IExecutionContextService CreateNullExecutionContextService()
        {
            var mock = Substitute.For<IExecutionContextService>();
            mock.CurrentContext.Returns((ExecutionContext?)null);

            return mock;
        }

        private async Task InitializeReadModelStoreConnectionAsync()
        {
            await this.readModelStoreContainer.StartAsync();

            this.readModelStoreConnection = new NpgsqlConnection(
                this.readModelStoreContainer.GetConnectionString());

            await this.readModelStoreConnection.OpenAsync();
        }
    }
}