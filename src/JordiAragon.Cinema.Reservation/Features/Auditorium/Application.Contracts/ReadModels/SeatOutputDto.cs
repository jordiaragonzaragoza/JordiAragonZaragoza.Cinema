namespace JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels
{
    using System;

    public record class SeatOutputDto(Guid Id, short Row, short SeatNumber, Guid AuditoriumId, string AuditoriumName);
}