namespace JordiAragon.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common
{
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;
    using Xunit;
    using Xunit.Abstractions;

    [Collection(nameof(SharedTestCollection))]
    public abstract class BaseEntityFrameworkIntegrationTests<TAggregate, TId> : IAsyncLifetime
        where TAggregate : BaseAggregateRoot<TId>
        where TId : class, IEntityId
    {
        protected BaseEntityFrameworkIntegrationTests(
            IntegrationTestsFixture fixture,
            ITestOutputHelper outputHelper)
        {
            this.Fixture = Guard.Against.Null(fixture, nameof(fixture));
            this.OutputHelper = Guard.Against.Null(outputHelper, nameof(outputHelper));
        }

        protected IntegrationTestsFixture Fixture { get; private init; }

        protected ITestOutputHelper OutputHelper { get; private init; }

        public virtual Task InitializeAsync()
        {
            this.Fixture.InitDatabase();

            return Task.CompletedTask;
        }

        public virtual async Task DisposeAsync()
            => await this.Fixture.ResetDatabaseAsync();

        protected ReservationRepository<TAggregate, TId> GetRepository()
            => new(this.Fixture.Context);
    }
}