namespace JordiAragon.Cinema.Application.Features.Auditorium.Showtime.Commands.CreateShowtime
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Showtime.Commands;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Specifications;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using Volo.Abp.Guids;

    public class CreateShowtimeCommandHandler : ICommandHandler<CreateShowtimeCommand, Guid>
    {
        private readonly IReadRepository<Movie> movieRepository;
        private readonly IRepository<Auditorium> auditoriumRepository;
        private readonly IGuidGenerator guidGenerator;

        public CreateShowtimeCommandHandler(
            IRepository<Auditorium> auditoriumRepository,
            IReadRepository<Movie> movieRepository,
            IGuidGenerator guidGenerator)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
            this.guidGenerator = Guard.Against.Null(guidGenerator, nameof(guidGenerator));
        }

        public async Task<Result<Guid>> Handle(CreateShowtimeCommand request, CancellationToken cancellationToken)
        {
            var existingAuditorium = await this.auditoriumRepository.FirstOrDefaultAsync(new AuditoriumWithShowtimesByIdSpec(AuditoriumId.Create(request.AuditoriumId)), cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {request.AuditoriumId} not found.");
            }

            var existingMovie = await this.movieRepository.GetByIdAsync(MovieId.Create(request.MovieId), cancellationToken);
            if (existingMovie is null)
            {
                return Result.NotFound($"{nameof(Movie)}: {request.MovieId} not found.");
            }

            var newShowtime = existingAuditorium.AddShowtime(
                ShowtimeId.Create(this.guidGenerator.Create()),
                existingMovie.Id,
                request.SessionDateOnUtc);

            await this.auditoriumRepository.UpdateAsync(existingAuditorium, cancellationToken);

            return Result.Success(newShowtime.Id.Value);
        }
    }
}