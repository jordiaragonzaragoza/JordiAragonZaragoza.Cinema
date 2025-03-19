namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class GetUserReservationsQuery(
        Guid UserId,
        Guid? ShowtimeId,
        DateTimeOffset? StartIntervalTimeOnUtc,
        DateTimeOffset? EndIntervalTimeOnUtc,
        string AuditoriumName,
        string MovieTitle,
        bool? IsPurchased,
        int PageNumber,
        int PageSize)
        : IPaginatedQuery, IQuery<PaginatedCollectionOutputDto<ReservationReadModel>>;
}