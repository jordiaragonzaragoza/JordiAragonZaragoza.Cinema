namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class PurchaseTicketCommand(Guid ShowtimeId, Guid TicketId) : ICommand;
}