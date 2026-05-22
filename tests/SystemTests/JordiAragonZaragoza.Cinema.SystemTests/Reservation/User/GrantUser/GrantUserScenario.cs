namespace JordiAragonZaragoza.Cinema.SystemTests.Reservation.User.GrantUser
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder;

    public sealed class GrantUserScenario
    {
        public Guid TenantId { get; } = SeedData.ExampleTenant.Id;

        public Guid? PartitionId { get; } = SeedData.ExamplePartition.Id;

        public Guid? CinemaId { get; } = SeedData.ExampleCinema.Id;

        public Guid UserId { get; } = SeedData.UserExampleToBeGranted.Id;

        public IReadOnlyList<string> Roles { get; } = new[] { JordiAragonZaragoza.Cinema.Reservation.Common.Application.Roles.Admin, JordiAragonZaragoza.Cinema.Reservation.Common.Application.Roles.Viewer };

        public IReadOnlyList<string> Permissions { get; } = [];

        public GrantUserBodyRequest GrantUserBodyRequest => new(
            this.TenantId,
            this.PartitionId,
            this.CinemaId,
            this.Roles,
            this.Permissions);

        public UserAuthorizationRequest UserAuthorizationRequest => new(
            this.UserId,
            this.TenantId,
            this.PartitionId,
            this.CinemaId);
    }
}
