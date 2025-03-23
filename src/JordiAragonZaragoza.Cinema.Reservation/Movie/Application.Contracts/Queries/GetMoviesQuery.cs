namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Queries
{
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class GetMoviesQuery(
            int PageNumber,
            int PageSize)
            : IPaginatedQuery, IQuery<PaginatedCollectionOutputDto<MovieReadModel>>;
}