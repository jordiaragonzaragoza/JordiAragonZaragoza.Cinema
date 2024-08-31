namespace JordiAragon.Cinema.Reservation.Movie.Application.CommandHanders.RemoveMovie
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.Movie.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public sealed class RemoveMovieCommandHandler : BaseCommandHandler<RemoveMovieCommand>
    {
        private readonly IRepository<Movie, MovieId> movieRepository;

        public RemoveMovieCommandHandler(IRepository<Movie, MovieId> movieRepository)
        {
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
        }

        public override async Task<Result> Handle(RemoveMovieCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var existingMovie = await this.movieRepository.GetByIdAsync(MovieId.Create(request.MovieId), cancellationToken);
            if (existingMovie is null)
            {
                return Result.NotFound($"{nameof(Movie)}: {request.MovieId} not found.");
            }

            existingMovie.Remove();

            await this.movieRepository.DeleteAsync(existingMovie, cancellationToken);

            return Result.Success();
        }
    }
}