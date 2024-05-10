namespace JordiAragon.Cinema.Reservation.Showtime.Domain.Rules
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.Cinema.Reservation.Movie.Domain;

    // TODO: This class will change on using sagas with timeout messages to mark showtimes as ended.
    public sealed class NoReservationsAllowedAfterShowtimeEndedRule : IBusinessRule
    {
        private readonly Showtime showtime;
        private readonly Movie movie;
        private readonly DateTimeOffset currentDateTimeOnUtc;

        public NoReservationsAllowedAfterShowtimeEndedRule(
            Showtime showtime,
            Movie movie,
            DateTimeOffset currentDateTimeOnUtc)
        {
            this.showtime = Guard.Against.Null(showtime, nameof(showtime));
            this.movie = Guard.Against.Null(movie, nameof(movie));
            this.currentDateTimeOnUtc = Guard.Against.Default(currentDateTimeOnUtc, nameof(currentDateTimeOnUtc));
        }

        public string Message => "No reservations are allowed after showtime ended.";

        public bool IsBroken()
        {
            var showtimeEndedOnUtc = this.showtime.SessionDateOnUtc + this.movie.Runtime;
            if (this.currentDateTimeOnUtc > showtimeEndedOnUtc)
            {
                return true;
            }

            return false;
        }
    }
}