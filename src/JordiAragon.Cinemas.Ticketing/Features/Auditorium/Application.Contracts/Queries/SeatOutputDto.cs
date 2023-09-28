namespace JordiAragon.Cinemas.Ticketing.Auditorium.Application.Contracts.Queries
{
    using System;

    public record class SeatOutputDto(Guid Id, short Row, short SeatNumber);
}