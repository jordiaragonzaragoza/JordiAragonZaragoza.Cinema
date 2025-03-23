namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class ReserveSeatsCommand(Guid ReservationId, Guid ShowtimeId, IEnumerable<Guid> SeatsIds) : ICommand<ReservationOutputDto>
    {
        public Guid UserId { get; init; } = new Guid("08ffddf5-3826-483f-a806-b3144477c7e8"); // TODO: Temporal till have authentication done.
    }
}