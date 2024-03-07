namespace JordiAragon.Cinema.Reservation.Showtime.Domain.Rules
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.Cinema.Reservation.Movie.Domain;

    public class NoReservationsAllowedAfterShowtimeEndedRule : IBusinessRule
    {
        private readonly Showtime showtime;
        private readonly DateTimeOffset createdTimeOnUtc;
        private readonly Movie movie;

        public NoReservationsAllowedAfterShowtimeEndedRule(
            Showtime showtime,
            Movie movie,
            DateTimeOffset createdTimeOnUtc)
        {
            this.showtime = Guard.Against.Null(showtime, nameof(showtime));
            this.movie = Guard.Against.Null(movie, nameof(movie));
            this.createdTimeOnUtc = Guard.Against.Default(createdTimeOnUtc, nameof(createdTimeOnUtc));
        }

        public string Message => "No reservations are allowed after showtime ended";

        public bool IsBroken()
        {
            var showtimeEndedOnUtc = this.showtime.SessionDateOnUtc + this.movie.Runtime;
            if (this.createdTimeOnUtc > showtimeEndedOnUtc)
            {
                return true;
            }

            return false;
        }
    }
}