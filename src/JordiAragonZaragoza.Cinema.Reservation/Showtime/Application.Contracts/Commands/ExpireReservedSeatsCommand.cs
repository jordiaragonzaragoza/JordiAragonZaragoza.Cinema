namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class ExpireReservedSeatsCommand(Guid ShowtimeId, Guid ReservationId) : ICommand;
}