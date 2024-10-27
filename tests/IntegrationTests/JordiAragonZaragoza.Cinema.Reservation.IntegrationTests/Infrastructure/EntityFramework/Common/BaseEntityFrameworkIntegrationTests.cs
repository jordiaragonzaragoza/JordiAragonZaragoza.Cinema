namespace JordiAragonZaragoza.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common
{
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.ReadModel;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;
    using Xunit;
    using Xunit.Abstractions;

    [Collection(nameof(SharedTestCollection))]
    public abstract class BaseEntityFrameworkIntegrationTests : IAsyncLifetime
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

        public virtual async Task InitializeAsync()
            => await this.Fixture.InitDatabasesAsync();

        public virtual async Task DisposeAsync()
            => await this.Fixture.ResetDatabasesAsync();

        protected ReservationRepository<TAggregate, TId> GetBusinessModelRepository<TAggregate, TId>()
            where TAggregate : BaseAggregateRoot<TId>
            where TId : class, IEntityId
            => new(this.Fixture.BusinessModelContext);

        protected ReservationReadModelRepository<TReadModel> GetReadModelRepository<TReadModel>()
            where TReadModel : class, IReadModel
            => new(this.Fixture.ReadModelContext);
    }
}