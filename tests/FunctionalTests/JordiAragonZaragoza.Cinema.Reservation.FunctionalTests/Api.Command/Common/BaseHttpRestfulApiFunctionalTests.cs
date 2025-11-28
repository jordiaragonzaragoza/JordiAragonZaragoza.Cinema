namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Command.Common
{
    using System;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query;
    using Xunit;
    using Xunit.Abstractions;

    [Collection(nameof(ApiCommandSharedTestCollection))]
    public abstract class BaseHttpRestfulApiFunctionalTests : IAsyncLifetime
    {
        protected BaseHttpRestfulApiFunctionalTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
        {
            this.Fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.OutputHelper = outputHelper ?? throw new ArgumentNullException(nameof(outputHelper));
        }

        protected FunctionalTestsFixture<Program> Fixture { get; private init; }

        protected ITestOutputHelper OutputHelper { get; private init; }

        public virtual async Task InitializeAsync()
            => await this.Fixture.InitDatabaseAsync();

        public virtual async Task DisposeAsync()
            => await this.Fixture.ResetDatabaseAsync();
    }
}