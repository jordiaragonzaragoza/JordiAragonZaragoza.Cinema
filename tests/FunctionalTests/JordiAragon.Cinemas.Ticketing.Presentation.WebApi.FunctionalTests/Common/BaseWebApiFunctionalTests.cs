namespace JordiAragon.Cinemas.Ticketing.Presentation.WebApi.FunctionalTests.Common
{
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;
    using Xunit.Abstractions;

    [Collection(nameof(SharedTestCollection))]
    public abstract class BaseWebApiFunctionalTests<TController> : IAsyncLifetime
        where TController : ControllerBase
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

        protected string ControllerBaseRoute
            => ControllerRouteHelpers.GetControllerBaseRoute<TController>();

        public virtual Task InitializeAsync()
        {
            this.Fixture.InitDatabase();

            return Task.CompletedTask;
        }

        public virtual async Task DisposeAsync()
            => await this.Fixture.ResetDatabaseAsync();

        protected string GetControllerMethodRoute(string methodName)
            => ControllerRouteHelpers.GetControllerMethodRoute<TController>(methodName);
    }
}