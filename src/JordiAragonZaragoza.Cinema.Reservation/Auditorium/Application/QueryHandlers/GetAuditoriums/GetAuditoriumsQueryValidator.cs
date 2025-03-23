namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.QueryHandlers.GetAuditoriums
{
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class GetAuditoriumsQueryValidator : BasePaginatedQueryValidator<GetAuditoriumsQuery>
    {
    }
}