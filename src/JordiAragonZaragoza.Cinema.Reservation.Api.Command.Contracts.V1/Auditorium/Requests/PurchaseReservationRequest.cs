namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1.Auditorium.Requests
{
    using System;

    public sealed record class PurchaseReservationRequest(Guid AuditoriumId, Guid ShowtimeId, Guid ReservationId, bool IsPurchased);
}