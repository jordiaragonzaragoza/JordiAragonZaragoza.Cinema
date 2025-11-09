namespace JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Auditorium.Requests
{
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;

    public sealed record class GetAuditoriumsRequest() : PaginatedRequest;
}