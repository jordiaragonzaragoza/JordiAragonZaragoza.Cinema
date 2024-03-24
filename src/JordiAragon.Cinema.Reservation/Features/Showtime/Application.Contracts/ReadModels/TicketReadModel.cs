namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class TicketReadModel(
        Guid Id,
        DateTimeOffset SessionDateOnUtc,
        Guid AuditoriumId,
        Guid MovieId,
        IEnumerable<SeatOutputDto> Seats,
        bool IsPurchased) : IReadModel;
}