namespace JordiAragonZaragoza.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common
{
    using System;
    using System.Linq;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain;

    public static class SeedData
    {
        public static readonly Tenant SystemTenant =
            Tenant.Create(
                id: new TenantId(SystemConstants.SystemTenantId));

        public static readonly TenantReadModel SystemTenantReadModel =
            new(SystemTenant.Id);

        public static readonly Partition ExamplePartition =
            Partition.Create(
                id: new PartitionId(new Guid("05cbd871-da4e-447b-9d62-89438df8c4a8")));

        public static readonly PartitionReadModel ExamplePartitionReadModel =
            new(ExamplePartition.Id);

        public static readonly Cinema ExampleCinema =
            Cinema.Create(
                id: new CinemaId(new Guid("497cd8f1-620c-426f-992d-7012147f6e7c")));

        public static readonly CinemaReadModel ExampleCinemaReadModel =
            new(ExampleCinema.Id);

        public static readonly Movie ExampleMovie =
            Movie.Add(
                id: new MovieId(new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")),
                title: Title.Create("Inception"),
                runtime: Runtime.Create(TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28)),
                exhibitionPeriod: ExhibitionPeriod.Create(
                    StartingPeriod.Create(new DateTimeOffset(DateTimeOffset.UtcNow.AddYears(1).Year, 1, 1, 1, 1, 1, TimeSpan.Zero)),
                    EndOfPeriod.Create(DateTimeOffset.UtcNow.AddYears(2)),
                    Runtime.Create(TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28))));

        public static readonly MovieReadModel ExampleMovieReadModel =
           new(
            ExampleMovie.Id,
            ExampleMovie.Title,
            ExampleMovie.Runtime);

        public static readonly Auditorium ExampleAuditorium =
            Auditorium.Create(
                id: new AuditoriumId(new Guid("c91aa0e0-9bc0-4db3-805c-23e3d8eabf53")),
                cinemaId: ExampleCinema.Id,
                name: Name.Create("Auditorium One"),
                rows: Rows.Create(10),
                seatsPerRow: SeatsPerRow.Create(10));

        public static readonly AuditoriumReadModel ExampleAuditoriumReadModel =
            new(
                ExampleAuditorium.Id,
                ExampleAuditorium.Name,
                ExampleAuditorium.CinemaId,
                ExampleAuditorium.Seats.Select(seat => new SeatReadModel(
                    seat.Id,
                    seat.Row,
                    seat.SeatNumber)).ToList());

        public static readonly User ExampleUser =
            User.Create(
                id: new UserId(new Guid("08ffddf5-3826-483f-a806-b3144477c7e8")));

        public static readonly UserReadModel ExampleUserReadModel =
            new(ExampleUser.Id);

        public static void PopulateReadModelTestData(ReservationReadModelContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            context.Tenants.Add(SystemTenantReadModel);

            context.Partitions.Add(ExamplePartitionReadModel);

            context.Cinemas.Add(ExampleCinemaReadModel);

            context.Movies.Add(ExampleMovieReadModel);

            context.Auditoriums.Add(ExampleAuditoriumReadModel);

            context.Users.Add(ExampleUserReadModel);

            context.SaveChanges();
        }
    }
}