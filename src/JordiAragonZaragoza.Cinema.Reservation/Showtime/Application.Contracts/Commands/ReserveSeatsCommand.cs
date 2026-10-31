namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class ReserveSeatsCommand(Guid ReservationId, Guid ShowtimeId, IEnumerable<Guid> SeatsIds) : ICommand<ReservationOutputDto>
    {
        public Guid UserId { get; init; } = new Guid("6bb46b99-e3d1-459b-9e1e-f8d8d7619864"); // TODO: Temporal till have authentication done.
    }
}