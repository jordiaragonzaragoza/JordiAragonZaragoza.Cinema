namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Projectors
{
    using Microsoft.Extensions.DependencyInjection;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Projectors.Movie;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Events;

    public static class MovieProjectorsEventHandlersDependencyInjection
    {
        public static IServiceCollection AddMovieProjectorsEventHandlers(this IServiceCollection services)
        {
            // Movie projection.
            services.AddProjectorEventHandler<MovieAddedEvent, MovieAddedEventProjector>();
            services.AddProjectorEventHandler<MovieRemovedEvent, MovieRemovedEventProjector>();

            return services;
        }
    }
}