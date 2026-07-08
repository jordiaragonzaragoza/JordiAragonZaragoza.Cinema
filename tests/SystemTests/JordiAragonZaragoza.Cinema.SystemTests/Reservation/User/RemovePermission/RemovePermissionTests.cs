namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.RemovePermission
{
    using System.Linq;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.Assertions;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class RemovePermissionTests : BaseSystemTests
    {
        public RemovePermissionTests(
            SystemTestsFixture fixture,
            ITestOutputHelper output)
            : base(fixture, output)
        {
        }

        [Fact]
        public async Task RemovePermission_ShouldRemovePermissionFromUserAuthorization()
        {
            var scenario = new RemovePermissionScenario();
            await scenario.ArrangeAsync(this.Fixture.ReservationCommandTestClient, this.Fixture.ReservationQueryTestClient, this.OutputHelper);

            await this.Fixture.ReservationCommandTestClient.RemovePermissionAsync(scenario.UserId, scenario.RemovePermissionBodyRequest, this.OutputHelper);

            await EventualConsistency.WaitUntilAsync(async () =>
            {
                var authorization = await this.Fixture.ReservationQueryTestClient.GetUserAuthorizationAsync(scenario.UserAuthorizationRequest, this.OutputHelper);
                return authorization is not null && !authorization.Permissions.Contains(scenario.PermissionToRemove);
            });

            await UserAuthorizationAssertions.ShouldNotContainPermissionAsync(
                this.Fixture.ReservationQueryTestClient,
                scenario.UserAuthorizationRequest,
                scenario.PermissionToRemove,
                this.OutputHelper);
        }
    }
}
