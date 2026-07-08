namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.GrantUser
{
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.Assertions;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class GrantUserTests : BaseSystemTests
    {
        public GrantUserTests(
            SystemTestsFixture fixture,
            ITestOutputHelper output)
            : base(fixture, output)
        {
        }

        [Fact]
        public async Task GrantUser_ShouldCreateUserAuthorizationWithRoles()
        {
            var scenario = new GrantUserScenario();

            await this.Fixture.ReservationCommandTestClient.GrantUserAsync(scenario.UserId, scenario.GrantUserBodyRequest, this.OutputHelper);

            await EventualConsistency.WaitUntilAsync(
                () => this.Fixture.ReservationQueryTestClient.UserAuthorizationExistsAsync(scenario.UserAuthorizationRequest, this.OutputHelper));

            await UserAuthorizationAssertions.ShouldContainRolesAsync(
                this.Fixture.ReservationQueryTestClient,
                scenario.UserAuthorizationRequest,
                scenario.Roles,
                this.OutputHelper);
        }
    }
}
