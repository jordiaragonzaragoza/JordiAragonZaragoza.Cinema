namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.RevokeUser
{
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.Assertions;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class RevokeUserTests : BaseSystemTests
    {
        public RevokeUserTests(
            SystemTestsFixture fixture,
            ITestOutputHelper output)
            : base(fixture, output)
        {
        }

        [Fact]
        public async Task RevokeUser_ShouldRemoveUserAuthorization()
        {
            var scenario = new RevokeUserScenario();
            await scenario.ArrangeAsync(this.Fixture.ReservationCommandTestClient, this.Fixture.ReservationQueryTestClient, this.OutputHelper);

            await this.Fixture.ReservationCommandTestClient.RevokeUserAsync(scenario.UserId, scenario.RevokeUserBodyRequest, this.OutputHelper);

            await EventualConsistency.WaitUntilAsync(
                () => this.Fixture.ReservationQueryTestClient.UserAuthorizationNotExistsAsync(scenario.UserAuthorizationRequest, this.OutputHelper));

            await UserAuthorizationAssertions.ShouldNotExistAsync(
                this.Fixture.ReservationQueryTestClient,
                scenario.UserAuthorizationRequest,
                this.OutputHelper);
        }
    }
}
