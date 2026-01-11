namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public sealed class ShowtimeHasToBeAlreadyRegisteredRule : IBusinessRule
    {
        private readonly IReadOnlyCollection<ShowtimeId> activeShowtimes;
        private readonly ShowtimeId existingActiveShowtimeId;

        public ShowtimeHasToBeAlreadyRegisteredRule(IReadOnlyCollection<ShowtimeId> activeShowtimes, ShowtimeId existingActiveShowtimeId)
        {
            this.activeShowtimes = activeShowtimes ?? throw new ArgumentNullException(nameof(activeShowtimes));
            this.existingActiveShowtimeId = existingActiveShowtimeId ?? throw new ArgumentNullException(nameof(existingActiveShowtimeId));
        }

        public string Message => $"Only Existing active showtimes can be removed. Showtime with id: {this.existingActiveShowtimeId} do not exists.";

        public bool IsBroken() => !this.activeShowtimes.Any(s => s == this.existingActiveShowtimeId);
    }
}