namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class ReserveSeatsCommand(Guid ReservationId, Guid ShowtimeId, IEnumerable<Guid> SeatsIds) : ICommand<ReservationOutputDto>
    {
        public Guid UserId { get; init; } = new Guid("737778f2-a3cd-4a2d-a784-cad6ffefa6ac"); // TODO: Temporal till have authentication done.
    }
}