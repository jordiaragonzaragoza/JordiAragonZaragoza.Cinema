namespace JordiAragon.Cinema.Reservation.Showtime.Application.CommandHandlers.ScheduleShowtime
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using Volo.Abp.Guids;

    public sealed class ScheduleShowtimeCommandHandler : BaseCommandHandler<ScheduleShowtimeCommand, Guid>
    {
        private readonly IReadRepository<Auditorium, AuditoriumId> auditoriumRepository;
        private readonly IReadRepository<Movie, MovieId> movieRepository;
        private readonly IRepository<Showtime, ShowtimeId> showtimeRepository;
        private readonly IGuidGenerator guidGenerator;

        public ScheduleShowtimeCommandHandler(
            IReadRepository<Auditorium, AuditoriumId> auditoriumRepository,
            IReadRepository<Movie, MovieId> movieRepository,
            IRepository<Showtime, ShowtimeId> showtimeRepository,
            IGuidGenerator guidGenerator)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
            this.guidGenerator = Guard.Against.Null(guidGenerator, nameof(guidGenerator));
        }

        public override async Task<Result<Guid>> Handle(ScheduleShowtimeCommand command, CancellationToken cancellationToken)
        {
            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(AuditoriumId.Create(command.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {command.AuditoriumId} not found.");
            }

            var existingMovie = await this.movieRepository.GetByIdAsync(MovieId.Create(command.MovieId), cancellationToken);
            if (existingMovie is null)
            {
                return Result.NotFound($"{nameof(Movie)}: {command.MovieId} not found.");
            }

            // TODO: This is temporal. A scheduler manager with business rules is required.
            var newShowtime = Showtime.Schedule(
                ShowtimeId.Create(this.guidGenerator.Create()),
                MovieId.Create(existingMovie.Id),
                command.SessionDateOnUtc,
                AuditoriumId.Create(existingAuditorium.Id));

            await this.showtimeRepository.AddAsync(newShowtime, cancellationToken);

            return Result.Success(newShowtime.Id.Value);
        }
    }
}