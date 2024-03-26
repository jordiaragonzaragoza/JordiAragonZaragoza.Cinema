namespace JordiAragon.Cinema.Reservation.User.Application.Contracts.Queries
{
    using System;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class GetUserTicketsQuery(
        Guid UserId,
        Guid? ShowtimeId,
        DateTimeOffset? StartIntervalTimeOnUtc,
        DateTimeOffset? EndIntervalTimeOnUtc,
        string AuditoriumName,
        string MovieTitle,
        bool? IsPurchased,
        int PageNumber,
        int PageSize)
        : IPaginatedQuery, IQuery<PaginatedCollectionOutputDto<TicketReadModel>>;
}