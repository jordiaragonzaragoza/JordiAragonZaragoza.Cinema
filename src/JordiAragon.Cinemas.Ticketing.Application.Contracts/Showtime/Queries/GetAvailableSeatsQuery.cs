namespace JordiAragon.Cinemas.Ticketing.Application.Contracts.Showtime.Queries
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Auditorium.Queries;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class GetAvailableSeatsQuery(Guid ShowtimeId) : IQuery<IEnumerable<SeatOutputDto>>;
}