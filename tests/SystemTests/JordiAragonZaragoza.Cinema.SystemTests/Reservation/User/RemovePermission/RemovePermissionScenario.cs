namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.RemovePermission
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

    public sealed class RemovePermissionScenario
    {
        public Guid TenantId { get; } = SeedData.ExampleTenant.Id;

        public Guid? PartitionId { get; } = SeedData.ExamplePartition.Id;

        public Guid? CinemaId { get; } = SeedData.ExampleCinema.Id;

        public Guid UserId { get; } = SeedData.ExampleUserToBeRemovedScheduleShowtimePermission.Id;

        public IReadOnlyList<string> BaseRoles { get; } = new[] { Roles.Admin };

        public string PermissionToRemove { get; } = ShowtimePermisions.ScheduleShowtime;

        public IReadOnlyList<string> Permissions { get; } = new[] { ShowtimePermisions.ScheduleShowtime };

        public GrantUserBodyRequest GrantUserBodyRequest => new(
            this.TenantId,
            this.PartitionId,
            this.CinemaId,
            this.BaseRoles,
            this.Permissions);

        public RemovePermissionBodyRequest RemovePermissionBodyRequest => new(
            this.TenantId,
            this.PartitionId,
            this.CinemaId,
            this.PermissionToRemove);

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

            output?.WriteLine($"Preparing user {this.UserId} with permissions {string.Join(',', this.Permissions)}");

            await commandClient.GrantUserAsync(this.UserId, this.GrantUserBodyRequest, output);

            await EventualConsistency.WaitUntilAsync(
                () => queryClient.UserAuthorizationExistsAsync(this.UserAuthorizationRequest, output));
        }
    }
}
