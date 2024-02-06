namespace JordiAragon.Cinema.Reservation.Showtime.Application.CommandHandlers.CreateShowtime
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Specifications;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Contracts.Repositories;
    using Volo.Abp.Guids;

    public class CreateShowtimeCommandHandler : BaseCommandHandler<CreateShowtimeCommand, Guid>
    {
        private readonly IRepository<Movie, MovieId> movieRepository;
        private readonly IRepository<Auditorium, AuditoriumId> auditoriumRepository;
        private readonly IRepository<Showtime, ShowtimeId> showtimeRepository;
        private readonly ISpecificationReadRepository<Showtime, ShowtimeId> showtimeReadRepository;
        private readonly IGuidGenerator guidGenerator;

        public CreateShowtimeCommandHandler(
            IRepository<Auditorium, AuditoriumId> auditoriumRepository,
            IRepository<Movie, MovieId> movieRepository,
            IRepository<Showtime, ShowtimeId> showtimeRepository,
            ISpecificationReadRepository<Showtime, ShowtimeId> showtimeReadRepository,
            IGuidGenerator guidGenerator)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
            this.showtimeReadRepository = Guard.Against.Null(showtimeReadRepository, nameof(showtimeReadRepository));
            this.guidGenerator = Guard.Against.Null(guidGenerator, nameof(guidGenerator));
        }

        public override async Task<Result<Guid>> Handle(CreateShowtimeCommand request, CancellationToken cancellationToken)
        {
            // TODO: Remove. This should be also in a domain rule.
            var existingShowtime = await this.showtimeReadRepository.FirstOrDefaultAsync(new ShowtimeByMovieIdSessionDateSpec(MovieId.Create(request.MovieId), request.SessionDateOnUtc), cancellationToken);
            if (existingShowtime is not null)
            {
                return Result.Invalid(new List<ValidationError>() { new ValidationError() { ErrorMessage = $"{nameof(Showtime)} already exists for this {nameof(Movie)}: {request.MovieId}" } });
            }

            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(AuditoriumId.Create(request.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {request.AuditoriumId} not found.");
            }

            var existingMovie = await this.movieRepository.GetByIdAsync(MovieId.Create(request.MovieId), cancellationToken);
            if (existingMovie is null)
            {
                return Result.NotFound($"{nameof(Movie)}: {request.MovieId} not found.");
            }

            var newShowtime = Showtime.Create(
                ShowtimeId.Create(this.guidGenerator.Create()),
                MovieId.Create(existingMovie.Id),
                request.SessionDateOnUtc,
                AuditoriumId.Create(existingAuditorium.Id));

            await this.showtimeRepository.AddAsync(newShowtime, cancellationToken);

            return Result.Success(newShowtime.Id.Value);
        }
    }
}