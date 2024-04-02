namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries
{
    using System;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class GetShowtimeTicketsQuery(
        Guid ShowtimeId,
        int PageNumber,
        int PageSize)
        : IPaginatedQuery, IQuery<PaginatedCollectionOutputDto<TicketReadModel>>;
}