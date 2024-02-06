namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels
{
    using System;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class ShowtimeReadModel(Guid Id, string MovieTitle, DateTimeOffset SessionDateOnUtc, Guid AuditoriumId, string AuditoriumName) : IReadModel;
}