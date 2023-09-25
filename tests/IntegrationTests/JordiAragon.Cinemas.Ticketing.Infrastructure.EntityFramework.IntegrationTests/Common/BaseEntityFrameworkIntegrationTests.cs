namespace JordiAragon.Cinemas.Ticketing.Infrastructure.EntityFramework.IntegrationTests.Common
{
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using Xunit;
    using Xunit.Abstractions;

    [Collection(nameof(SharedTestCollection))]
    public abstract class BaseEntityFrameworkIntegrationTests<TAggregate> : IAsyncLifetime
        where TAggregate : class, IAggregateRoot
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

        protected CinemaRepository<TAggregate> GetRepository()
            => new(this.Fixture.Context);
    }
}