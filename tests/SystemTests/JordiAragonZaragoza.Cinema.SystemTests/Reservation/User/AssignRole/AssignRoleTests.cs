namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.AssignRole
{
    using System.Linq;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.Assertions;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class AssignRoleTests : BaseSystemTests
    {
        public AssignRoleTests(
            SystemTestsFixture fixture,
            ITestOutputHelper output)
            : base(fixture, output)
        {
        }

        [Fact]
        public async Task AssignRole_ShouldAddRoleToExistingUserAuthorization()
        {
            var scenario = new AssignRoleScenario();
            await scenario.ArrangeAsync(this.Fixture.ReservationCommandTestClient, this.Fixture.ReservationQueryTestClient, this.OutputHelper);

            await this.Fixture.ReservationCommandTestClient.AssignRoleAsync(scenario.UserId, scenario.AssignRoleBodyRequest, this.OutputHelper);

            await EventualConsistency.WaitUntilAsync(async () =>
            {
                var authorization = await this.Fixture.ReservationQueryTestClient.GetUserAuthorizationAsync(scenario.UserAuthorizationRequest, this.OutputHelper);
                return authorization is not null && authorization.Roles.Contains(scenario.RoleToAssign);
            });

            await UserAuthorizationAssertions.ShouldContainRolesAsync(
                this.Fixture.ReservationQueryTestClient,
                scenario.UserAuthorizationRequest,
                scenario.ExpectedRoles,
                this.OutputHelper);
        }
    }
}
