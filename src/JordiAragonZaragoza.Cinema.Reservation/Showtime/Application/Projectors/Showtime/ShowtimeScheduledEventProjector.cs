namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.Showtime
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ShowtimeScheduledEventProjector : BaseEventHandler<ShowtimeScheduledEvent>
    {
        private readonly IReadRepository<AuditoriumReadModel, Guid> auditoriumReadModelRepository;
        private readonly IReadRepository<MovieReadModel, Guid> movieReadModelRepository;
        private readonly IRepository<ShowtimeReadModel, Guid> showtimeReadModelRepository;

        public ShowtimeScheduledEventProjector(
            IReadRepository<AuditoriumReadModel, Guid> auditoriumReadModelRepository,
            IReadRepository<MovieReadModel, Guid> movieReadModelRepository,
            IRepository<ShowtimeReadModel, Guid> showtimeReadModelRepository)
        {
            this.auditoriumReadModelRepository = auditoriumReadModelRepository ?? throw new ArgumentNullException(nameof(auditoriumReadModelRepository));
            this.movieReadModelRepository = movieReadModelRepository ?? throw new ArgumentNullException(nameof(movieReadModelRepository));
            this.showtimeReadModelRepository = showtimeReadModelRepository ?? throw new ArgumentNullException(nameof(showtimeReadModelRepository));
        }

        public override async Task HandleAsync(ShowtimeScheduledEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var existingAuditorium = await this.auditoriumReadModelRepository.GetByIdAsync(@event.AuditoriumId, cancellationToken);
            if (existingAuditorium is null)
            {
                throw new NotFoundException(nameof(Auditorium), @event.AuditoriumId.ToString());
            }

            var existingMovie = await this.movieReadModelRepository.GetByIdAsync(@event.MovieId, cancellationToken);
            if (existingMovie is null)
            {
                throw new NotFoundException(nameof(Movie), @event.MovieId.ToString());
            }

            var showtimeReadModel = new ShowtimeReadModel(
                @event.AggregateId,
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