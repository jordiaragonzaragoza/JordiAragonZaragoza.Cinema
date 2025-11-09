namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Movie.Requests
{
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public sealed record class GetMoviesRequest() : PaginatedRequest;
}