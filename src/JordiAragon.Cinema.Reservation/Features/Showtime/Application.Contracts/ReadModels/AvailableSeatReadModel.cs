namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class AvailableSeatReadModel(
        Guid Id,
        Guid SeatId,
        short Row,
        short SeatNumber,
        Guid ShowtimeId,
        Guid AuditoriumId,
        string AuditoriumName) : IReadModel;
}