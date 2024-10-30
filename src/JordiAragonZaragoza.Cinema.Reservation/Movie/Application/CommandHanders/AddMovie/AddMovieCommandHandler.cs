namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHanders.AddMovie
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Commands;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class AddMovieCommandHandler : BaseCommandHandler<AddMovieCommand>
    {
        private readonly IRepository<Movie, MovieId> movieRepository;

        public AddMovieCommandHandler(IRepository<Movie, MovieId> movieRepository)
        {
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
        }

        public override async Task<Result> Handle(AddMovieCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            // TODO: There cannot be two movies with the same title, runtime and exhibitionPeriod.
            // Check will be done via domain service.
            var newMovie = Movie.Add(
                id: MovieId.Create(request.MovieId),
                title: request.Title,
                runtime: Runtime.Create(request.Runtime),
                exhibitionPeriod: ExhibitionPeriod.Create(
                    StartingPeriod.Create(request.StartingPeriod),
                    EndOfPeriod.Create(request.EndOfPeriod),
                    Runtime.Create(request.Runtime)));

            await this.movieRepository.AddAsync(newMovie, cancellationToken);

            return Result.Success();
        }
    }
}