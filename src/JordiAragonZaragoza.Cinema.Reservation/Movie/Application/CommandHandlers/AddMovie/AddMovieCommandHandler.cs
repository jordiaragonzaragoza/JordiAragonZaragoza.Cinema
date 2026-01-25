namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.AddMovie
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class AddMovieCommandHandler : ICommandHandler<AddMovieCommand>
    {
        private readonly IRepository<Movie, MovieId> movieRepository;

        public AddMovieCommandHandler(IRepository<Movie, MovieId> movieRepository)
        {
            this.movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
        }

        public async Task<Result> Handle(AddMovieCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            // TODO: There cannot be two movies with the same title, runtime and exhibitionPeriod.
            // Check will be done via domain service.
            var newMovie = Movie.Add(
                id: new MovieId(request.MovieId),
                title: Title.Create(request.Title),
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