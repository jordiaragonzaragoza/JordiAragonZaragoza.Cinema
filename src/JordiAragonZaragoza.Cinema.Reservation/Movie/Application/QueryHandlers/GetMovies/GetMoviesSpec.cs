namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.QueryHandlers.GetMovies
{
    using System;
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
            this.request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public IPaginatedQuery Request
            => this.request;
    }
}