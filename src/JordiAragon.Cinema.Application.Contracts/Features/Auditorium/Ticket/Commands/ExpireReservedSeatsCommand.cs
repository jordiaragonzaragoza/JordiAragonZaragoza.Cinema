namespace JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Ticket.Commands
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class ExpireReservedSeatsCommand(Guid TicketId) : ICommand; // TODO: ShowtimeId will be required.
}