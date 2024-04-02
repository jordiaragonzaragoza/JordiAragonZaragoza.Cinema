namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests
{
    using System;
    using JordiAragon.SharedKernel.Presentation.WebApi.Contracts;

    public sealed record class GetShowtimeTicketsRequest(
        Guid ShowtimeId)
        : PaginatedRequest;
}