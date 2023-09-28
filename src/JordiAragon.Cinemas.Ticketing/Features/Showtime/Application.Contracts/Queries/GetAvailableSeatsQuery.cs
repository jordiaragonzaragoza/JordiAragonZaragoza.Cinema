namespace JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Queries
{
    using System;
    using System.Collections.Generic;
    using JordiAragon.Cinemas.Ticketing.Auditorium.Application.Contracts.Queries;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class GetAvailableSeatsQuery(Guid ShowtimeId) : IQuery<IEnumerable<SeatOutputDto>>;
}