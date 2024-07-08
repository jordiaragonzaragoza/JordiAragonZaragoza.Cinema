namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Xunit;
    using Xunit.Abstractions;

    [Collection(nameof(SharedTestCollection))]
    public abstract class BaseHttpRestfulApiFunctionalTests : IAsyncLifetime
    {
        protected BaseHttpRestfulApiFunctionalTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
        {
            this.Fixture = Guard.Against.Null(fixture, nameof(fixture));
            this.OutputHelper = Guard.Against.Null(outputHelper, nameof(outputHelper));
        }

        protected FunctionalTestsFixture<Program> Fixture { get; private init; }

        protected ITestOutputHelper OutputHelper { get; private init; }

        public virtual async Task InitializeAsync()
            => await this.Fixture.InitDatabasesAsync();

        public virtual async Task DisposeAsync()
            => await this.Fixture.ResetDatabasesAsync();

        protected static async Task AddEventualConsistencyDelayAsync()
            => await Task.Delay(TimeSpan.FromSeconds(2));
    }
}