namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.QueryHandlers.GetMovies
{
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Validators;

    public sealed class GetMoviesQueryValidator : BasePaginatedQueryValidator<GetMoviesQuery>
    {
    }
}