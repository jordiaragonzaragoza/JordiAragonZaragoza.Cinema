namespace JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Ticket.Commands
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class ReserveSeatsCommand : ICommand<TicketOutputDto>
    {
        public Guid AuditoriumId { get; set; } // This set is required to be wired post mapping.

        public Guid ShowtimeId { get; set; } // This set is required to be wired post mapping.

        public IEnumerable<Guid> SeatsIds { get; init; }
    }
}