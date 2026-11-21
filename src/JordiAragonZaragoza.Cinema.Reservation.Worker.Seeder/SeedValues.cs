namespace JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder
{
    using System;

    /// <summary>
    /// Deterministic values of the seed data.
    /// They are immutable, so they can be safely shared between processes, tests and HTTP files.
    /// Tests and requests should reference these ids instead of reading them from <see cref="SeedData"/>.
    /// </summary>
    public static class SeedValues
    {
        public const string ExampleMovieTitle = "Inception";

        public const string ExampleAuditoriumName = "Auditorium One";

        public const int ExampleAuditoriumRows = 10;

        public const int ExampleAuditoriumSeatsPerRow = 10;

        public static readonly Guid ExampleTenantId = new("667196fc-0e17-43f9-990d-7dc2175e6162");

        public static readonly Guid ExamplePartitionId = new("05cbd871-da4e-447b-9d62-89438df8c4a8");

        public static readonly Guid ExampleCinemaId = new("497cd8f1-620c-426f-992d-7012147f6e7c");

        public static readonly Guid ExampleMovieId = new("3fa85f64-5717-4562-b3fc-2c963f66afa6");

        public static readonly Guid ExampleAuditoriumId = new("c91aa0e0-9bc0-4db3-805c-23e3d8eabf53");

        public static readonly Guid ExampleShowtimeId = new("89b073a7-cfcf-4f2a-b01b-4c7f71a0563b");

        public static readonly Guid ExampleReservationId = new("d290f1ee-6c54-4b01-90e6-d701748f0851");

        public static readonly Guid ExampleUserId = new("b7dedad7-4c6b-498e-8808-bd88468ca97f");

        public static readonly Guid ExampleUserWithReservationId = new("a9c483ba-4f95-400b-835d-1a9c6c39e17b");

        public static readonly Guid ExampleUserWithAdminRoleId = new("6bb46b99-e3d1-459b-9e1e-f8d8d7619864");

        public static readonly Guid ExampleUserToBeGrantedId = new("7acabd24-d196-4599-b053-083b4fb8ba50");

        public static readonly Guid ExampleUserToBeAssignedAsViewerRoleId = new("d6551436-94f4-4275-9660-efacec34e90a");

        public static readonly Guid ExampleUserToBeRevokedId = new("82e78b83-24b0-4036-b3c0-b44f830c9442");

        public static readonly Guid ExampleUserToBeRemovedAsViewerRoleId = new("cd782d72-90c2-4373-b6c4-be9857c80bba");

        public static readonly Guid ExampleUserToBeAssignedScheduleShowtimePermissionId = new("e7662547-a5d7-4386-a7d5-cf0a8dc91ccb");

        public static readonly Guid ExampleUserToBeRemovedScheduleShowtimePermissionId = new("f8773658-b6e8-4497-b8e6-db1b9ed02ddc");
    }
}
