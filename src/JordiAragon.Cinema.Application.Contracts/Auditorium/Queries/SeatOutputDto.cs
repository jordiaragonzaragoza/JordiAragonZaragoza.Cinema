namespace JordiAragon.Cinema.Application.Contracts.Auditorium.Queries
{
    using System;

    public record class SeatOutputDto(Guid Id, short Row, short SeatNumber);
}