﻿namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class PurchaseReservationCommand(Guid ShowtimeId, Guid ReservationId) : ICommand;
}