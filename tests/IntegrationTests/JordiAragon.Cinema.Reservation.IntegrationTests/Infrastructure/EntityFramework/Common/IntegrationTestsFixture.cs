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
        private readonly SqlEdgeContainer writeStoreContainer =
            new SqlEdgeBuilder()
            .WithImage("mcr.microsoft.com/azure-sql-edge:latest")
            .WithName("azuresqledge.cinema.reservation.writestore.integrationtests.infrastructure.entityframework")
            .WithAutoRemove(true).Build();

        private SqlConnection writeStoreConnection;
        private Respawner writeStoreRespawner;

        public ReservationWriteContext WriteContext { get; private set; }

        public async Task InitializeAsync()
        {
            await this.writeStoreContainer.StartAsync();

            this.writeStoreConnection = new SqlConnection(this.writeStoreContainer.GetConnectionString());

            this.writeStoreRespawner = await Respawner.CreateAsync(this.writeStoreContainer.GetConnectionString(), new RespawnerOptions
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

            this.WriteContext = new ReservationWriteContext(options, mockLoggerFactory, mockHostEnvironment, auditableEntitySaveChangesInterceptor);

            SeedData.PopulateWriteTestData(this.WriteContext);
        }

        public async Task ResetDatabasesAsync()
        {
            await this.writeStoreRespawner.ResetAsync(this.writeStoreContainer.GetConnectionString());
        }

        public async Task DisposeAsync()
        {
            await this.writeStoreConnection.DisposeAsync();
            await this.writeStoreContainer.DisposeAsync();
        }

        private DbContextOptions<ReservationWriteContext> CreateNewWriteContextOptions()
        {
            // Create a new options instance telling the context to use an
            var builder = new DbContextOptionsBuilder<ReservationWriteContext>();
            builder.UseSqlServer(this.writeStoreConnection);

            return builder.Options;
        }
    }
}