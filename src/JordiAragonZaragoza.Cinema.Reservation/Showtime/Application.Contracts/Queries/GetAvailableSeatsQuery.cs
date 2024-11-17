namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class GetAvailableSeatsQuery(Guid ShowtimeId) : IQuery<IEnumerable<AvailableSeatReadModel>>;
}