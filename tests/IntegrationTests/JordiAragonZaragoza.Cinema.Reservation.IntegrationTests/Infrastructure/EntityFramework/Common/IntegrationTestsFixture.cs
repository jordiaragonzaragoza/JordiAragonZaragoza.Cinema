namespace JordiAragonZaragoza.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common
{
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.SharedKernel;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
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
            .WithName("postgres.cinema.reservation.readmodelstore.integrationtests.infrastructure.entityframework")
            .WithAutoRemove(true).Build();

        private NpgsqlConnection readModelStoreConnection = default!;
        private Respawner readModelStoreRespawner = default!;

        public ReservationReadModelContext ReadModelContext { get; private set; } = default!;

        public async Task InitializeAsync()
        {
            await this.InitializeReadModelStoreConnectionAsync();
        }

        public async Task InitDatabasesAsync()
        {
            await this.InitReadModelStoreDatabaseAsync();
        }

        public async Task ResetDatabasesAsync()
        {
            await this.readModelStoreRespawner.ResetAsync(this.readModelStoreConnection);
        }

        public async Task DisposeAsync()
        {
            await this.readModelStoreConnection.DisposeAsync();
            await this.readModelStoreContainer.DisposeAsync();
        }

        private async Task InitializeReadModelStoreConnectionAsync()
        {
            await this.readModelStoreContainer.StartAsync();

            this.readModelStoreConnection = new NpgsqlConnection(this.readModelStoreContainer.GetConnectionString());
            await this.readModelStoreConnection.OpenAsync();
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