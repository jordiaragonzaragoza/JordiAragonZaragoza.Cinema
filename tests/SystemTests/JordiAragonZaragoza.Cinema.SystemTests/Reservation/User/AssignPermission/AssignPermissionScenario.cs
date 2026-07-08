namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.AssignPermission
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Application;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime;
    using Xunit.Abstractions;

    public sealed class AssignPermissionScenario
    {
        public Guid TenantId { get; } = SeedData.ExampleTenant.Id;

        public Guid? PartitionId { get; } = SeedData.ExamplePartition.Id;

        public Guid? CinemaId { get; } = SeedData.ExampleCinema.Id;

        public Guid UserId { get; } = SeedData.ExampleUserToBeAssignedScheduleShowtimePermission.Id;

        public IReadOnlyList<string> ExpectedPermissions { get; } = new[] { ShowtimePermisions.ScheduleShowtime };

        public IReadOnlyList<string> BaseRoles { get; } = new[] { Roles.Admin };

        public IReadOnlyList<string> Permissions { get; } = [];

        public string PermissionToAssign { get; } = ShowtimePermisions.ScheduleShowtime;

        public GrantUserBodyRequest GrantUserBodyRequest => new(
            this.TenantId,
            this.PartitionId,
            this.CinemaId,
            this.BaseRoles,
            this.Permissions);

        public AssignPermissionBodyRequest AssignPermissionBodyRequest => new(
            this.TenantId,
            this.PartitionId,
            this.CinemaId,
            this.PermissionToAssign);

        public UserAuthorizationRequest UserAuthorizationRequest => new(
            this.UserId,
            this.TenantId,
            this.PartitionId,
            this.CinemaId);

        public async Task ArrangeAsync(
            ReservationCommandTestClient commandClient,
            ReservationQueryTestClient queryClient,
            ITestOutputHelper? output = null)
        {
            ArgumentNullException.ThrowIfNull(commandClient);
            ArgumentNullException.ThrowIfNull(queryClient);

            output?.WriteLine($"Preparing user {this.UserId} with base roles {string.Join(',', this.BaseRoles)}");

            await commandClient.GrantUserAsync(this.UserId, this.GrantUserBodyRequest, output);

            await EventualConsistency.WaitUntilAsync(
                () => queryClient.UserAuthorizationExistsAsync(this.UserAuthorizationRequest, output));
        }
    }
}
