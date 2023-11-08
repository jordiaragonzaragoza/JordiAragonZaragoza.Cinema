namespace JordiAragon.Cinema.Reservation.Showtime.Application.Queries.GetShowtimes
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain.Specifications;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Specifications;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class GetShowtimesQueryHandler : IQueryHandler<GetShowtimesQuery, IEnumerable<ShowtimeOutputDto>>
    {
        private readonly IReadRepository<Movie, MovieId> movieRepository;
        private readonly IReadRepository<Showtime, ShowtimeId> showtimeRepository;

        public GetShowtimesQueryHandler(
            IReadRepository<Movie, MovieId> movieRepository,
            IReadRepository<Showtime, ShowtimeId> showtimeRepository)
        {
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
        }

        public async Task<Result<IEnumerable<ShowtimeOutputDto>>> Handle(GetShowtimesQuery request, CancellationToken cancellationToken)
        {
            var specification = new ShowtimesByAuditoriumIdSpec(
                AuditoriumId.Create(request.AuditoriumId),
                request.StartTimeOnUtc,
                request.EndTimeOnUtc,
                request.MovieId.HasValue ? MovieId.Create(request.MovieId.Value) : null);

            var existingShowtimes = await this.showtimeRepository.ListAsync(specification, cancellationToken);
            if (!existingShowtimes.Any())
            {
                return Result.NotFound($"There is not any {nameof(Showtime)} avaliable.");
            }

            var showtimeOutputDtos = new List<ShowtimeOutputDto>();

            foreach (var showtime in existingShowtimes)
            {
                var movie = await this.movieRepository.FirstOrDefaultAsync(new MovieByIdSpec(showtime.MovieId), cancellationToken);
                if (movie is null)
                {
                    return Result.NotFound($"{nameof(Movie)}: {showtime.MovieId.Value} not found.");
                }

                showtimeOutputDtos.Add(new ShowtimeOutputDto(showtime.Id.Value, movie.Title, showtime.SessionDateOnUtc, request.AuditoriumId));
            }

            return Result.Success(showtimeOutputDtos.AsEnumerable());
        }
    }
}