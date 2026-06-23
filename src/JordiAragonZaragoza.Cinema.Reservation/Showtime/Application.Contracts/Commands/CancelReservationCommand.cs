namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Application;
    using JordiAragonZaragoza.SharedKernel.Application.Attributes;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    [Authorize(Policies = Policies.SelfOrAdmin)]
    public sealed record CancelReservationCommand(
        Guid ShowtimeId,
        Guid ReservationId) : ICommand, IPolicyResourceRequest
    {
        public Guid ResourceId => this.ReservationId;
    }
}