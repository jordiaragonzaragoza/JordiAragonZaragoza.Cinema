namespace JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Seat.Queries
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class GetAvailableSeatsQuery(Guid AuditoriumId, Guid ShowtimeId) : IQuery<IEnumerable<SeatOutputDto>>;
}