namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class OnlyContiguousSeatsCanBeReservedRule : IBusinessRule
    {
        private readonly IEnumerable<Seat> desiredSeats;

        public OnlyContiguousSeatsCanBeReservedRule(IEnumerable<Seat> desiredSeats)
        {
            this.desiredSeats = Guard.Against.NullOrEmpty(desiredSeats, nameof(desiredSeats));
        }

        public string Message => "Only contiguous seats can be reserved.";

        public bool IsBroken()
        {
            var sortedSeats = this.desiredSeats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber).ToList();

            for (int i = 0; i < sortedSeats.Count - 1; i++)
            {
                var currentSeat = sortedSeats[i];
                var nextSeat = sortedSeats[i + 1];

                if (currentSeat.Row != nextSeat.Row || currentSeat.SeatNumber != nextSeat.SeatNumber - 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}