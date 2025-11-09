namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.User.Requests
{
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public sealed record class GetUsersRequest() : PaginatedRequest;
}