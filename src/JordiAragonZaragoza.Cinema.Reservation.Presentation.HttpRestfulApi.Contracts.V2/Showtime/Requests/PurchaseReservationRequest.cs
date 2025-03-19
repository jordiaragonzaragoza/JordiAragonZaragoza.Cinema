namespace JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests
{
    using System;

    public sealed record class PurchaseReservationRequest(Guid ShowtimeId, Guid ReservationId, bool IsPurchased);
}