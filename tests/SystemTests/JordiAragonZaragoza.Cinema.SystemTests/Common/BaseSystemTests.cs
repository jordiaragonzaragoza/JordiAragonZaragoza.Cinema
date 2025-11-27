namespace JordiAragonZaragoza.Cinema.SystemTests.Common
{
    using System;
    using System.Threading.Tasks;
    using Xunit;
    using Xunit.Abstractions;

    [Collection(nameof(SharedTestCollection))]
    public abstract class BaseSystemTests
    {
        protected BaseSystemTests(
            SystemTestsFixture fixture,
            ITestOutputHelper outputHelper)
        {
            this.Fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            this.OutputHelper = outputHelper ?? throw new ArgumentNullException(nameof(outputHelper));
        }

        protected SystemTestsFixture Fixture { get; private init; }

        protected ITestOutputHelper OutputHelper { get; private init; }

        protected static async Task AddEventualConsistencyDelayAsync()
            => await Task.Delay(TimeSpan.FromSeconds(2));
    }
}