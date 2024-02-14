namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class ShowtimeReadModel(
        Guid Id,
        DateTimeOffset SessionDateOnUtc,
        Guid MovieId,
        string MovieTitle,
        TimeSpan MovieRuntime,
        Guid AuditoriumId,
        string AuditoriumName) : IReadModel;
}