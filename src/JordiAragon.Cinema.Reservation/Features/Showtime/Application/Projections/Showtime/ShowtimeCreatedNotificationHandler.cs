namespace JordiAragon.Cinema.Reservation.Showtime.Application.Projections.Showtime
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Notifications;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using MediatR;

    public class ShowtimeCreatedNotificationHandler : INotificationHandler<ShowtimeCreatedNotification>
    {
        private readonly IRepository<Auditorium, AuditoriumId> auditoriumRepository;
        private readonly IRepository<Movie, MovieId> movieRepository;
        private readonly IRepository<ShowtimeReadModel, Guid> showtimeReadModelRepository;

        public ShowtimeCreatedNotificationHandler(
            IRepository<Auditorium, AuditoriumId> auditoriumRepository,
            IRepository<Movie, MovieId> movieRepository,
            IRepository<ShowtimeReadModel, Guid> showtimeReadModelRepository)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
            this.showtimeReadModelRepository = Guard.Against.Null(showtimeReadModelRepository, nameof(showtimeReadModelRepository));
        }

        public async Task Handle(ShowtimeCreatedNotification notification, CancellationToken cancellationToken)
        {
            var @event = notification.Event;

            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(AuditoriumId.Create(@event.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                throw new NotFoundException(nameof(Auditorium), @event.AuditoriumId.ToString());
            }

            var existingMovie = await this.movieRepository.GetByIdAsync(MovieId.Create(@event.MovieId), cancellationToken);
            if (existingMovie is null)
            {
                throw new NotFoundException(nameof(Movie), @event.MovieId.ToString());
            }

            var showtimeReadModel = new ShowtimeReadModel(
                @event.ShowtimeId,
                @event.SessionDateOnUtc,
                existingMovie.Id,
                existingMovie.Title,
                existingMovie.Runtime,
                @event.AuditoriumId,
                existingAuditorium.Name);

            await this.showtimeReadModelRepository.AddAsync(showtimeReadModel, cancellationToken);
        }
    }
}