namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.Queries
{
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;

    public sealed record class GetCinemasQuery(
        int PageNumber,
        int PageSize)
            : IPaginatedQuery, IQuery<PaginatedCollectionOutputDto<CinemaReadModel>>;
}