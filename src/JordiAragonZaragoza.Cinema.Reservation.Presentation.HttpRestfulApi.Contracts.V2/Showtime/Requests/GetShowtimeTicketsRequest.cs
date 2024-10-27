namespace JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public sealed record class GetShowtimeTicketsRequest(
        Guid ShowtimeId)
        : PaginatedRequest;
}