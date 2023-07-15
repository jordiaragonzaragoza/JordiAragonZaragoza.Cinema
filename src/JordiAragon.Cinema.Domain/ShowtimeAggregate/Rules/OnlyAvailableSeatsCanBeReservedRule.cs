namespace JordiAragon.Cinema.Domain.ShowtimeAggregate.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class OnlyAvailableSeatsCanBeReservedRule : IBusinessRule
    {
        private readonly IEnumerable<Seat> desiredSeats;
        private readonly IEnumerable<Seat> availableSeats;

        public OnlyAvailableSeatsCanBeReservedRule(IEnumerable<Seat> desiredSeats, IEnumerable<Seat> availableSeats)
        {
            this.desiredSeats = Guard.Against.NullOrEmpty(desiredSeats, nameof(desiredSeats));
            this.availableSeats = Guard.Against.NullOrEmpty(availableSeats, nameof(availableSeats));
        }

        public string Message => $"Only available seats can be reserved:\n{string.Join(",\n", this.availableSeats.Select(seatId => seatId.Id.Value))}";

        public bool IsBroken() => !this.desiredSeats.Select(seat => seat.Id).All(seatId => this.availableSeats.Select(seat => seat.Id).Contains(seatId));
    }
}