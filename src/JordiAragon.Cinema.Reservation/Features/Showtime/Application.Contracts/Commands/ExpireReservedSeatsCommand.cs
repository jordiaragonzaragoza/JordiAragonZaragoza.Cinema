namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class ExpireReservedSeatsCommand(Guid ShowtimeId, Guid TicketId) : ICommand;
}