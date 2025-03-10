﻿namespace JordiAragonZaragoza.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common
{
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework;
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
        private readonly PostgreSqlContainer businessModelStoreContainer =
            new PostgreSqlBuilder()
            .WithImage("postgres:15.1-alpine")
            .WithName("postgres.cinema.reservation.businessmodelstore.integrationtests.infrastructure.entityframework")
            .WithAutoRemove(true).Build();

        private readonly PostgreSqlContainer readModelStoreContainer =
            new PostgreSqlBuilder()
            .WithImage("postgres:15.1-alpine")
            .WithName("postgres.cinema.reservation.readmodelstore.integrationtests.infrastructure.entityframework")
            .WithAutoRemove(true).Build();

        private NpgsqlConnection businessModelStoreConnection = default!;
        private Respawner businessModelStoreRespawner = default!;

        private NpgsqlConnection readModelStoreConnection = default!;
        private Respawner readModelStoreRespawner = default!;

        public ReservationBusinessModelContext BusinessModelContext { get; private set; } = default!;

        public ReservationReadModelContext ReadModelContext { get; private set; } = default!;

        public async Task InitializeAsync()
        {
            var initializeBusinessModelStoreTask = this.InitializeBusinessModelStoreConnectionAsync();
            var initializeReadModelStoreTask = this.InitializeReadModelStoreConnectionAsync();

            await Task.WhenAll(initializeBusinessModelStoreTask, initializeReadModelStoreTask);
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
            var disposeBusinessModelConnectionTask = this.businessModelStoreConnection.DisposeAsync();
            var disposeReadModelConnectionTask = this.readModelStoreConnection.DisposeAsync();

            await Task.WhenAll(disposeBusinessModelConnectionTask.AsTask(), disposeReadModelConnectionTask.AsTask());

            var disposeBusinessModelContainerTask = this.businessModelStoreContainer.DisposeAsync();
            var disposeReadModelContainerTask = this.readModelStoreContainer.DisposeAsync();

            await Task.WhenAll(disposeBusinessModelContainerTask.AsTask(), disposeReadModelContainerTask.AsTask());
        }

        private async Task InitializeBusinessModelStoreConnectionAsync()
        {
            await this.businessModelStoreContainer.StartAsync();

            this.businessModelStoreConnection = new NpgsqlConnection(this.businessModelStoreContainer.GetConnectionString());
            await this.businessModelStoreConnection.OpenAsync();
        }

        private async Task InitializeReadModelStoreConnectionAsync()
        {
            await this.readModelStoreContainer.StartAsync();

            this.readModelStoreConnection = new NpgsqlConnection(this.readModelStoreContainer.GetConnectionString());
            await this.readModelStoreConnection.OpenAsync();
        }

        private async Task InitBusinessModelStoreDatabaseAsync()
        {
            var options = this.CreateNewBusinessModelContextOptions();
            var mockLoggerFactory = Substitute.For<ILoggerFactory>();
            var mockHostEnvironment = Substitute.For<IHostEnvironment>();

            var softDeleteEntitySaveChangesInterceptor = new SoftDeleteEntitySaveChangesInterceptor();

            this.BusinessModelContext = new ReservationBusinessModelContext(options, mockLoggerFactory, mockHostEnvironment, softDeleteEntitySaveChangesInterceptor);

            await this.BusinessModelContext.Database.MigrateAsync();
            SeedData.PopulateBusinessModelTestData(this.BusinessModelContext);

            this.businessModelStoreRespawner = await Respawner.CreateAsync(this.businessModelStoreConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
            });
        }

        private DbContextOptions<ReservationBusinessModelContext> CreateNewBusinessModelContextOptions()
        {
            // Create a new options instance telling the context to use an
            var builder = new DbContextOptionsBuilder<ReservationBusinessModelContext>();
            builder.UseNpgsql(this.businessModelStoreConnection);

            return builder.Options;
        }

        private async Task InitReadModelStoreDatabaseAsync()
        {
            var options = this.CreateNewReadModelContextOptions();
            var mockLoggerFactory = Substitute.For<ILoggerFactory>();
            var mockHostEnvironment = Substitute.For<IHostEnvironment>();

            this.ReadModelContext = new ReservationReadModelContext(options, mockLoggerFactory, mockHostEnvironment);

            await this.ReadModelContext.Database.MigrateAsync();
            SeedData.PopulateReadModelTestData(this.ReadModelContext);

            this.readModelStoreRespawner = await Respawner.CreateAsync(this.readModelStoreConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
            });
        }

        private DbContextOptions<ReservationReadModelContext> CreateNewReadModelContextOptions()
        {
            // Create a new options instance telling the context to use an
            var builder = new DbContextOptionsBuilder<ReservationReadModelContext>();
            builder.UseNpgsql(this.readModelStoreConnection);

            return builder.Options;
        }
    }
}