namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.QueryHandlers.GetMovies
{
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed class GetMoviesSpec : Specification<MovieReadModel>, IPaginatedSpecification<MovieReadModel>
    {
        private readonly GetMoviesQuery request;

        public GetMoviesSpec(GetMoviesQuery request)
        {
            this.request = Guard.Against.Null(request);
        }

        public IPaginatedQuery Request
            => this.request;
    }
}