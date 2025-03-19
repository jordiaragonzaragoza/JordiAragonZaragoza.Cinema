namespace JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1.Auditorium.Showtime.Requests
{
    using System;

    public sealed record class PurchaseReservationRequest(Guid AuditoriumId, Guid ShowtimeId, Guid ReservationId, bool IsPurchased);
}