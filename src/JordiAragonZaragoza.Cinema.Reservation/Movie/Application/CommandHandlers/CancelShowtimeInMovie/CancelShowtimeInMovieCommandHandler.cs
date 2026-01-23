namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.CancelShowtimeInMovie
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;

    public sealed class CancelShowtimeInMovieCommandHandler : ICommandHandler<CancelShowtimeInMovieCommand>
    {
        private readonly IRepository<Movie, MovieId> movieRepository;

        public CancelShowtimeInMovieCommandHandler(IRepository<Movie, MovieId> movieRepository)
        {
            this.movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
        }

        public async Task<Result> Handle(CancelShowtimeInMovieCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var existingMovie = await this.movieRepository.GetByIdAsync(new MovieId(request.MovieId), cancellationToken)
                                    ?? throw new NotFoundException(nameof(Movie), request.MovieId.ToString());

            existingMovie.CancelShowtime(new ShowtimeId(request.ShowtimeId));

            await this.movieRepository.UpdateAsync(existingMovie, cancellationToken);

            return Result.NoContent();
        }
    }
}