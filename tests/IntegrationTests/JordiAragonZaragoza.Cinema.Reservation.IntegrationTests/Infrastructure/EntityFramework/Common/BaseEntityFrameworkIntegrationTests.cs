namespace JordiAragonZaragoza.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using NSubstitute;
    using Xunit;
    using Xunit.Abstractions;

    using ExecutionContext = JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces.ExecutionContext;

    [Collection(nameof(EntityFrameworkSharedTestCollection))]
    public abstract class BaseEntityFrameworkIntegrationTests : IAsyncLifetime
    {
        private static readonly SemaphoreSlim InitSemaphore = new(1, 1);
        private static bool databaseInitialized;

        protected BaseEntityFrameworkIntegrationTests(
            IntegrationTestsFixture fixture,
            ITestOutputHelper outputHelper)
        {
            this.Fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.OutputHelper = outputHelper ?? throw new ArgumentNullException(nameof(outputHelper));
        }

        protected IntegrationTestsFixture Fixture { get; }

        protected ITestOutputHelper OutputHelper { get; }

        protected ReservationReadModelContext ReadModelContext { get; private set; } = default!;

        protected IExecutionContextService ExecutionContextService { get; private set; } = default!;

        public virtual async Task InitializeAsync()
        {
            await InitSemaphore.WaitAsync();
            try
            {
                if (!databaseInitialized)
                {
                    await this.Fixture.EnsureDatabaseInitializedAsync();
                    databaseInitialized = true;
                }
            }
            finally
            {
                InitSemaphore.Release();
            }

            await this.Fixture.ResetDatabasesAsync();

            this.ExecutionContextService = this.CreateExecutionContextService();
            this.ReadModelContext = this.Fixture.CreateReadModelContext(this.ExecutionContextService);

            await this.SeedAsync();
        }

        public virtual async Task DisposeAsync()
        {
            await this.ReadModelContext.DisposeAsync();
        }

        /// <summary>
        /// Seeds the database with test data. Can be overridden to seed specific data for a test.
        /// This method is called after the database has been reset and before each test runs.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected virtual Task SeedAsync()
        {
            SeedData.PopulateReadModelTestData(this.ReadModelContext);

            return Task.CompletedTask;
        }

        protected ReservationReadModelRepository<TReadModel> GetReadModelRepository<TReadModel>()
            where TReadModel : class, IReadModel
            => new(this.ReadModelContext);

        /// <summary>
        /// Creates an ExecutionContextService for integration tests.
        /// Can be overridden to use a different tenant.
        /// </summary>
        /// <returns> A IExecutionContextService for integration tests.</returns>
        protected virtual IExecutionContextService CreateExecutionContextService()
        {
            var testContext = new ExecutionContext(
                actorId: ExecutionContext.CreateServiceActorId("integration-tests"),
                actorType: ActorType.System,
                executor: nameof(BaseEntityFrameworkIntegrationTests),
                executorType: ExecutorType.Tool,
                correlationId: Guid.CreateVersion7(),
                causationId: null,
                scopeContext: new ScopeContext(
                    tenantId: SystemConstants.SystemTenantId,
                    partitionId: null,
                    domainId: null));

            var mock = Substitute.For<IExecutionContextService>();
            mock.CurrentContext.Returns(testContext);

            return mock;
        }
    }
}