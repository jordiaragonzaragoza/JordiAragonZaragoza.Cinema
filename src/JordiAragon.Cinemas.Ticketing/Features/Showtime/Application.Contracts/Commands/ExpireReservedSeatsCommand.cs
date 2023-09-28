namespace JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Commands
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class ExpireReservedSeatsCommand(Guid ShowtimeId, Guid TicketId) : ICommand;
}