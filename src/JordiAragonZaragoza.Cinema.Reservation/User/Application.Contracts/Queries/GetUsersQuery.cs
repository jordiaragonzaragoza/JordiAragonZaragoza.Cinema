namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries
{
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;

    public sealed record class GetUsersQuery(
        int PageNumber,
        int PageSize)
            : IPaginatedQuery, IQuery<PaginatedCollectionOutputDto<UserReadModel>>;
}