namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.AssignPermission
{
    using System.Linq;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.Assertions;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class AssignPermissionTests : BaseSystemTests
    {
        public AssignPermissionTests(
            SystemTestsFixture fixture,
            ITestOutputHelper output)
            : base(fixture, output)
        {
        }

        [Fact]
        public async Task AssignPermission_ShouldAddPermissionToExistingUserAuthorization()
        {
            var scenario = new AssignPermissionScenario();
            await scenario.ArrangeAsync(this.Fixture.ReservationCommandTestClient, this.Fixture.ReservationQueryTestClient, this.OutputHelper);

            await this.Fixture.ReservationCommandTestClient.AssignPermissionAsync(scenario.UserId, scenario.AssignPermissionBodyRequest, this.OutputHelper);

            await EventualConsistency.WaitUntilAsync(async () =>
            {
                var authorization = await this.Fixture.ReservationQueryTestClient.GetUserAuthorizationAsync(scenario.UserAuthorizationRequest, this.OutputHelper);
                return authorization is not null && authorization.Permissions.Contains(scenario.PermissionToAssign);
            });

            await UserAuthorizationAssertions.ShouldContainPermissionsAsync(
                this.Fixture.ReservationQueryTestClient,
                scenario.UserAuthorizationRequest,
                scenario.ExpectedPermissions,
                this.OutputHelper);
        }
    }
}
