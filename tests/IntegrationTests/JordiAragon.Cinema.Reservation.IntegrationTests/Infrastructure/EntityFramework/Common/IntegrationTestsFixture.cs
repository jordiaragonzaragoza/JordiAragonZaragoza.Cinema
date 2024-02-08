namespace JordiAragon.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common
{
    using System.Threading.Tasks;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Interceptors;
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using Respawn;
    using Testcontainers.SqlEdge;
    using Xunit;

    public class IntegrationTestsFixture : IAsyncLifetime
    {
        private readonly SqlEdgeContainer businessModelStoreContainer =
            new SqlEdgeBuilder()
            .WithImage("mcr.microsoft.com/azure-sql-edge:latest")
            .WithName("azuresqledge.cinema.reservation.businessmodelstore.integrationtests.infrastructure.entityframework")
            .WithAutoRemove(true).Build();

        private readonly SqlEdgeContainer readModelStoreContainer =
            new SqlEdgeBuilder()
            .WithImage("mcr.microsoft.com/azure-sql-edge:latest")
            .WithName("azuresqledge.cinema.reservation.readmodelstore.integrationtests.infrastructure.entityframework")
            .WithAutoRemove(true).Build();

        private SqlConnection businessModelStoreConnection;
        private Respawner businessModelStoreRespawner;

        private SqlConnection readModelStoreConnection;
        private Respawner readModelStoreRespawner;

        public ReservationBusinessModelContext BusinessModelContext { get; private set; }

        public ReservationReadModelContext ReadModelContext { get; private set; }

        public async Task InitializeAsync()
        {
            var initializeBusinessModelStoreTask = this.InitializeBusinessModelStoreConnectionAsync();
            var initializeReadModelStoreTask = this.InitializeReadModelStoreConnectionAsync();

            await Task.WhenAll(initializeBusinessModelStoreTask, initializeReadModelStoreTask);
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

        private async Task InitializeBusinessModelStoreConnectionAsync()
        {
            await this.businessModelStoreContainer.StartAsync();

            this.businessModelStoreConnection = new SqlConnection(this.businessModelStoreContainer.GetConnectionString());

            this.businessModelStoreRespawner = await Respawner.CreateAsync(this.businessModelStoreContainer.GetConnectionString(), new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
            });
        }

        private async Task InitializeReadModelStoreConnectionAsync()
        {
            await this.readModelStoreContainer.StartAsync();

            this.readModelStoreConnection = new SqlConnection(this.readModelStoreContainer.GetConnectionString());

            this.readModelStoreRespawner = await Respawner.CreateAsync(this.readModelStoreContainer.GetConnectionString(), new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
            });
        }

        private void InitBusinessModelStoreDatabase()
        {
            var options = this.CreateNewBusinessModelContextOptions();
            var mockLoggerFactory = Substitute.For<ILoggerFactory>();
            var mockHostEnvironment = Substitute.For<IHostEnvironment>();
            var mockCurrentUserService = Substitute.For<ICurrentUserService>();
            var mockDateTimeService = Substitute.For<IDateTime>();

            var auditableEntitySaveChangesInterceptor = new AuditableEntitySaveChangesInterceptor(mockCurrentUserService, mockDateTimeService);

            this.BusinessModelContext = new ReservationBusinessModelContext(options, mockLoggerFactory, mockHostEnvironment, auditableEntitySaveChangesInterceptor);

            SeedData.PopulateBusinessModelTestData(this.BusinessModelContext);
        }

        private DbContextOptions<ReservationBusinessModelContext> CreateNewBusinessModelContextOptions()
        {
            // Create a new options instance telling the context to use an
            var builder = new DbContextOptionsBuilder<ReservationBusinessModelContext>();
            builder.UseSqlServer(this.businessModelStoreConnection);

            return builder.Options;
        }

        private void InitReadModelStoreDatabase()
        {
            var options = this.CreateNewReadModelContextOptions();
            var mockLoggerFactory = Substitute.For<ILoggerFactory>();
            var mockHostEnvironment = Substitute.For<IHostEnvironment>();

            this.ReadModelContext = new ReservationReadModelContext(options, mockLoggerFactory, mockHostEnvironment);

            SeedData.PopulateBusinessModelTestData(this.BusinessModelContext);
        }

        private DbContextOptions<ReservationReadModelContext> CreateNewReadModelContextOptions()
        {
            // Create a new options instance telling the context to use an
            var builder = new DbContextOptionsBuilder<ReservationReadModelContext>();
            builder.UseSqlServer(this.readModelStoreConnection);

            return builder.Options;
        }
    }
}