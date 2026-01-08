namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class OnlyNonExistingActiveShowtimeCanBeAddedRule : IBusinessRule
    {
        private readonly IReadOnlyCollection<ShowtimeId> activeShowtimes;
        private readonly ShowtimeId newActiveShowtimeId;

        public OnlyNonExistingActiveShowtimeCanBeAddedRule(IReadOnlyCollection<ShowtimeId> activeShowtimes, ShowtimeId newActiveShowtimeId)
        {
            this.activeShowtimes = activeShowtimes ?? throw new ArgumentNullException(nameof(activeShowtimes));
            this.newActiveShowtimeId = newActiveShowtimeId ?? throw new ArgumentNullException(nameof(newActiveShowtimeId));
        }

        public string Message => $"Only non existing active showtimes can be added. Showtime with id: {this.newActiveShowtimeId} already exists.";

        public bool IsBroken() => this.activeShowtimes.Any(s => s == this.newActiveShowtimeId);
    }
}