namespace JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Queries
{
    using System;

    public record class SeatOutputDto(Guid Id, short Row, short SeatNumber, Guid AuditoriumId, string AuditoriumName);
}