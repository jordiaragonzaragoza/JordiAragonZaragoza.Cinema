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

        private SqlConnection businessModelStoreConnection;
        private Respawner businessModelStoreRespawner;

        public ReservationBusinessModelContext BusinessModelContext { get; private set; }

        public async Task InitializeAsync()
        {
            await this.businessModelStoreContainer.StartAsync();

            this.businessModelStoreConnection = new SqlConnection(this.businessModelStoreContainer.GetConnectionString());

            this.businessModelStoreRespawner = await Respawner.CreateAsync(this.businessModelStoreContainer.GetConnectionString(), new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
            });
        }

        public void InitDatabases()
        {
            var options = this.CreateNewWriteContextOptions();
            var mockLoggerFactory = Substitute.For<ILoggerFactory>();
            var mockHostEnvironment = Substitute.For<IHostEnvironment>();
            var mockCurrentUserService = Substitute.For<ICurrentUserService>();
            var mockDateTimeService = Substitute.For<IDateTime>();

            var auditableEntitySaveChangesInterceptor = new AuditableEntitySaveChangesInterceptor(mockCurrentUserService, mockDateTimeService);

            this.BusinessModelContext = new ReservationBusinessModelContext(options, mockLoggerFactory, mockHostEnvironment, auditableEntitySaveChangesInterceptor);

            SeedData.PopulateBusinessModelTestData(this.BusinessModelContext);
        }

        public async Task ResetDatabasesAsync()
        {
            await this.businessModelStoreRespawner.ResetAsync(this.businessModelStoreContainer.GetConnectionString());
        }

        public async Task DisposeAsync()
        {
            await this.businessModelStoreConnection.DisposeAsync();
            await this.businessModelStoreContainer.DisposeAsync();
        }

        private DbContextOptions<ReservationBusinessModelContext> CreateNewWriteContextOptions()
        {
            // Create a new options instance telling the context to use an
            var builder = new DbContextOptionsBuilder<ReservationBusinessModelContext>();
            builder.UseSqlServer(this.businessModelStoreConnection);

            return builder.Options;
        }
    }
}