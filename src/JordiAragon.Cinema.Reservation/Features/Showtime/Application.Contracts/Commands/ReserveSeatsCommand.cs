namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class ReserveSeatsCommand(Guid ShowtimeId, IEnumerable<Guid> SeatsIds) : ICommand<TicketOutputDto>;
}