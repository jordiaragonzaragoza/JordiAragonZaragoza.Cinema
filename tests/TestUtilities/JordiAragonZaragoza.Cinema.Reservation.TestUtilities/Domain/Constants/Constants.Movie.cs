namespace JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;

    public static partial class Constants
    {
        public static class Movie
        {
            public static readonly Title Title = Title.Create("Inception");
            public static readonly MovieId Id = new(new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"));
            public static readonly Runtime Runtime = Runtime.Create(TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28));
            public static readonly StartingPeriod StartingPeriod = StartingPeriod.Create(new DateTimeOffset(DateTimeOffset.UtcNow.AddYears(1).Year, 1, 1, 1, 1, 1, 1, TimeSpan.Zero));
            public static readonly EndOfPeriod EndOfPeriod = EndOfPeriod.Create(DateTimeOffset.UtcNow.AddYears(2));
            public static readonly ExhibitionPeriod ExhibitionPeriod = ExhibitionPeriod.Create(
                    StartingPeriod,
                    EndOfPeriod,
                    Runtime);
        }
    }
}