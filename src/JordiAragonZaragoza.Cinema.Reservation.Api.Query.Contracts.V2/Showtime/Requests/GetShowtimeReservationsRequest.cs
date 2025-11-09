namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Requests
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public sealed record class GetShowtimeReservationsRequest(
        Guid ShowtimeId)
        : PaginatedRequest;
}