namespace JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TicketSeats")]
    public sealed record class SeatReadModel(Guid Id, short Row, short SeatNumber);
}