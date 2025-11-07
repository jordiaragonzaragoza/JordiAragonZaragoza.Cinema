namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.QueryHandlers
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.QueryHandlers.GetMovies;

    public static class MovieQueryHandlersDependencyInjection
    {
        public static IServiceCollection AddMovieQueryHandlers(this IServiceCollection services)
        {
            services.AddQueryHandler<GetMoviesQuery, PaginatedCollectionOutputDto<MovieReadModel>, GetMoviesQueryHandler>();

            return services;
        }
    }
}