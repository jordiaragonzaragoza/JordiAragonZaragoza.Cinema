namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Application;
    using JordiAragonZaragoza.SharedKernel.Application.Attributes;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    ////[Authorize(Roles = Roles.Admin, Permissions = ShowtimePermisions.Cancel)]
    public sealed record class CancelShowtimeCommand(Guid ShowtimeId) : ICommand;
}