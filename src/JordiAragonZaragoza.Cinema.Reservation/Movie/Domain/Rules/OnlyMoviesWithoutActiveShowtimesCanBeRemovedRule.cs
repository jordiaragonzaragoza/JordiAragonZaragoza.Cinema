namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class OnlyMoviesWithoutActiveShowtimesCanBeRemovedRule : IBusinessRule
    {
        private readonly IReadOnlyCollection<ShowtimeId> activeShowtimes;

        public OnlyMoviesWithoutActiveShowtimesCanBeRemovedRule(IReadOnlyCollection<ShowtimeId> activeShowtimes)
        {
            this.activeShowtimes = activeShowtimes ?? throw new ArgumentNullException(nameof(activeShowtimes));
        }

        public string Message => $"Only auditoriums without active showtimes can be removed.";

        public bool IsBroken() => this.activeShowtimes.Count > 0;
    }
}