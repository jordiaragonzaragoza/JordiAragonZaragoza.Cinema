namespace JordiAragonZaragoza.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;

    public static class SeedData
    {
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
            ExampleMovie.Id.Value,
            ExampleMovie.Title.Value,
            ExampleMovie.Runtime.Value);

        public static readonly Auditorium ExampleAuditorium =
            Auditorium.Create(
                id: new AuditoriumId(new Guid("c91aa0e0-9bc0-4db3-805c-23e3d8eabf53")),
                name: Name.Create("Auditorium One"),
                rows: Rows.Create(10),
                seatsPerRow: SeatsPerRow.Create(10));

        public static readonly AuditoriumReadModel ExampleAuditoriumReadModel =
            new(
                ExampleAuditorium.Id.Value,
                ExampleAuditorium.Name.Value);

        public static readonly User ExampleUser =
            User.Create(
                id: new UserId(new Guid("08ffddf5-3826-483f-a806-b3144477c7e8")));

        public static void PopulateBusinessModelTestData(ReservationBusinessModelContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            context.Movies.Add(ExampleMovie);

            context.Auditoriums.Add(ExampleAuditorium);

            context.Users.Add(ExampleUser);

            context.SaveChanges();
        }

        public static void PopulateReadModelTestData(ReservationReadModelContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            context.Movies.Add(ExampleMovieReadModel);

            context.Auditoriums.Add(ExampleAuditoriumReadModel);

            context.SaveChanges();
        }
    }
}