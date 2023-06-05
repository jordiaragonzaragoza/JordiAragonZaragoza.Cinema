namespace JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Seat.Queries
{
    using System;

    public record class SeatOutputDto(Guid Id, short Row, short SeatNumber);
}