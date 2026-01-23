namespace JordiAragonZaragoza.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common
{
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using Xunit;
    using Xunit.Abstractions;

    [Collection(nameof(EntityFrameworkSharedTestCollection))]
    public abstract class BaseEntityFrameworkIntegrationTests : IAsyncLifetime
    {
        protected BaseEntityFrameworkIntegrationTests(
            IntegrationTestsFixture fixture,
            ITestOutputHelper outputHelper)
        {
            this.Fixture = fixture ?? throw new System.ArgumentNullException(nameof(fixture));
            this.OutputHelper = outputHelper ?? throw new System.ArgumentNullException(nameof(outputHelper));
        }

        protected IntegrationTestsFixture Fixture { get; private init; }

        protected ITestOutputHelper OutputHelper { get; private init; }

        public virtual async Task InitializeAsync()
            => await this.Fixture.InitDatabasesAsync();

        public virtual async Task DisposeAsync()
            => await this.Fixture.ResetDatabasesAsync();

        protected ReservationReadModelRepository<TReadModel> GetReadModelRepository<TReadModel>()
            where TReadModel : class, IReadModel
            => new(this.Fixture.ReadModelContext);
    }
}