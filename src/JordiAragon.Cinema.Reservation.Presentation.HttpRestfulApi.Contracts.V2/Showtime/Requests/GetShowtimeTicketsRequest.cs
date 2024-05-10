namespace JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests
{
    using System;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public sealed record class GetShowtimeTicketsRequest(
        Guid ShowtimeId)
        : PaginatedRequest;
}