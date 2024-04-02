namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.WebApi.Common
{
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Xunit;
    using Xunit.Abstractions;

    [Collection(nameof(SharedTestCollection))]
    public abstract class BaseWebApiFunctionalTests : IAsyncLifetime
    {
        protected BaseWebApiFunctionalTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
        {
            this.Fixture = Guard.Against.Null(fixture, nameof(fixture));
            this.OutputHelper = Guard.Against.Null(outputHelper, nameof(outputHelper));
        }

        protected FunctionalTestsFixture<Program> Fixture { get; private init; }

        protected ITestOutputHelper OutputHelper { get; private init; }

        public virtual Task InitializeAsync()
        {
            this.Fixture.InitDatabases();

            return Task.CompletedTask;
        }

        public virtual async Task DisposeAsync()
            => await this.Fixture.ResetDatabasesAsync();
    }
}