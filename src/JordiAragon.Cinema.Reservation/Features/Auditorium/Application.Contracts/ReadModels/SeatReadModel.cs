namespace JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels
{
    using System;

    public sealed record class SeatReadModel(Guid Id, short Row, short SeatNumber);
}