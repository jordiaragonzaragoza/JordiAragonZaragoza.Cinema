namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.RemoveRole
{
    using System.Linq;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.Assertions;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class RemoveRoleTests : BaseSystemTests
    {
        public RemoveRoleTests(
            SystemTestsFixture fixture,
            ITestOutputHelper output)
            : base(fixture, output)
        {
        }

        [Fact]
        public async Task RemoveRole_ShouldRemoveRoleFromUserAuthorization()
        {
            var scenario = new RemoveRoleScenario();
            await scenario.ArrangeAsync(this.Fixture.ReservationCommandTestClient, this.Fixture.ReservationQueryTestClient, this.OutputHelper);

            await this.Fixture.ReservationCommandTestClient.RemoveRoleAsync(scenario.UserId, scenario.RemoveRoleBodyRequest, this.OutputHelper);

            await EventualConsistency.WaitUntilAsync(async () =>
            {
                var authorization = await this.Fixture.ReservationQueryTestClient.GetUserAuthorizationAsync(scenario.UserAuthorizationRequest, this.OutputHelper);
                return authorization is not null && !authorization.Roles.Contains(scenario.RoleToRemove);
            });

            await UserAuthorizationAssertions.ShouldNotContainRoleAsync(
                this.Fixture.ReservationQueryTestClient,
                scenario.UserAuthorizationRequest,
                scenario.RoleToRemove,
                this.OutputHelper);
        }
    }
}
