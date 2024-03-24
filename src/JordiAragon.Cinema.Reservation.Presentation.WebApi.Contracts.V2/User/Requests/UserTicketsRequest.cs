namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.User.Requests
{
    using System;
    using JordiAragon.SharedKernel.Presentation.WebApi.Contracts;

    public record class UserTicketsRequest(Guid UserId, Guid ShowtimeId) : PaginatedRequest;
}