namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class GetUserReservationQuery(Guid UserId, Guid ShowtimeId, Guid ReservationId) : IQuery<ReservationReadModel>;
}