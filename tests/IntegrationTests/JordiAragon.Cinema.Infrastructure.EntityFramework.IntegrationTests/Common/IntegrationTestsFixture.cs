﻿namespace JordiAragon.Cinema.Infrastructure.EntityFramework.IntegrationTests.Common
{
    using System.Threading.Tasks;
    using JordiAragon.Cinema.Infrastructure.EntityFramework;
    using JordiAragon.Cinema.Infrastructure.EntityFramework.AssemblyConfiguration;
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
        private readonly SqlEdgeContainer container =
            new SqlEdgeBuilder()
            .WithImage("mcr.microsoft.com/azure-sql-edge:latest")
            .WithName("azuresqledge.cinema.infrastructure.entityframework.integrationtests")
            .WithAutoRemove(true).Build();

        private SqlConnection connection;
        private Respawner respawner;

        public CinemaContext Context { get; private set; }

        public async Task InitializeAsync()
        {
            await this.container.StartAsync();

            this.connection = new SqlConnection(this.container.GetConnectionString());

            this.respawner = await Respawner.CreateAsync(this.container.GetConnectionString(), new RespawnerOptions
            {
                TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
            });
        }

        public void InitDatabase()
        {
            var options = this.CreateNewContextOptions();
            var mockLoggerFactory = Substitute.For<ILoggerFactory>();
            var mockHostEnvironment = Substitute.For<IHostEnvironment>();
            var mockCurrentUserService = Substitute.For<ICurrentUserService>();
            var mockDateTimeService = Substitute.For<IDateTime>();

            var auditableEntitySaveChangesInterceptor = new AuditableEntitySaveChangesInterceptor(mockCurrentUserService, mockDateTimeService);

            this.Context = new CinemaContext(options, mockLoggerFactory, mockHostEnvironment, auditableEntitySaveChangesInterceptor);

            SeedData.PopulateTestData(this.Context);
        }

        public async Task ResetDatabaseAsync()
        {
            await this.respawner.ResetAsync(this.container.GetConnectionString());
        }

        public async Task DisposeAsync()
        {
            await this.connection.DisposeAsync();
            await this.container.DisposeAsync();
        }

        private DbContextOptions<CinemaContext> CreateNewContextOptions()
        {
            // Create a new options instance telling the context to use an
            var builder = new DbContextOptionsBuilder<CinemaContext>();
            builder.UseSqlServer(this.connection);

            return builder.Options;
        }
    }
}