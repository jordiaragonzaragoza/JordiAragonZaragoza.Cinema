namespace JordiAragon.Cinema.Reservation.Showtime.Domain.Specifications
{
    using System;
    using System.Linq;
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;

    public class CurrentAndPreviousDayNotEndedShowtimesSpec : Specification<Showtime>
    {
        public CurrentAndPreviousDayNotEndedShowtimesSpec(
            DateTimeOffset currentDateTimeOnUtc,
            AuditoriumId auditoriumId = null,
            MovieId movieId = null)
        {
            Guard.Against.Default(currentDateTimeOnUtc);

            this.Query
                .Where(s => !s.IsEnded)
                .Where(s => s.AuditoriumId == auditoriumId, auditoriumId is not null)
                .Where(s => s.MovieId == movieId, movieId is not null)
                .Where(showtime => showtime.SessionDateOnUtc < currentDateTimeOnUtc
                                   && (showtime.SessionDateOnUtc.Date.AddDays(-1) == currentDateTimeOnUtc.Date
                                    || showtime.SessionDateOnUtc.Date == currentDateTimeOnUtc.Date));
        }
    }
}