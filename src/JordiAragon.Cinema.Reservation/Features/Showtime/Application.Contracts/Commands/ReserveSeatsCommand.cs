namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class ReserveSeatsCommand(Guid ShowtimeId, IEnumerable<Guid> SeatsIds) : ICommand<TicketOutputDto>
    {
        public Guid UserId { get; init; } = new Guid("08ffddf5-3826-483f-a806-b3144477c7e8"); // TODO: Temporal till have authentication done.
    }
}