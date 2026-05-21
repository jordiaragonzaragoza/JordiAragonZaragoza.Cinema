namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.RevokeUser
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder;
    using JordiAragonZaragoza.Cinema.SystemTests.Common;
    using JordiAragonZaragoza.Cinema.SystemTests.Reservation.Showtime;
    using Xunit.Abstractions;

    public sealed class RevokeUserScenario
    {
        public Guid TenantId { get; } = SeedData.ExampleTenant.Id;

        public Guid? PartitionId { get; } = SeedData.ExamplePartition.Id;

        public Guid? CinemaId { get; } = SeedData.ExampleCinema.Id;

        public Guid UserId { get; } = SeedData.ExampleUserToBeRevoked.Id;

        public IReadOnlyList<string> Roles { get; } = new[] { "Admin" };

        public IReadOnlyList<string> Permissions { get; } = [];

        public GrantUserBodyRequest GrantUserBodyRequest => new(
            this.TenantId,
            this.PartitionId,
            this.CinemaId,
            this.Roles,
            this.Permissions);

        public RevokeUserBodyRequest RevokeUserBodyRequest => new(
            this.TenantId,
            this.PartitionId,
            this.CinemaId);

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

            output?.WriteLine($"Preparing user {this.UserId} with roles {string.Join(',', this.Roles)}");

            await commandClient.GrantUserAsync(this.UserId, this.GrantUserBodyRequest, output);

            await EventualConsistency.WaitUntilAsync(
                () => queryClient.UserAuthorizationExistsAsync(this.UserAuthorizationRequest, output));
        }
    }
}
