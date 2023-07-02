namespace JordiAragon.Cinema.Application.Contracts.Features.Showtime.Queries
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Queries;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class GetAvailableSeatsQuery(Guid ShowtimeId) : IQuery<IEnumerable<SeatOutputDto>>;
}