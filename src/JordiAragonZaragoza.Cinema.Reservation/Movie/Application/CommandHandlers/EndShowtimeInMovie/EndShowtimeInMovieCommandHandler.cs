namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Application.CommandHandlers.EndShowtimeInMovie
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;

    public sealed class EndShowtimeInMovieCommandHandler : ICommandHandler<EndShowtimeInMovieCommand>
    {
        private readonly IRepository<Auditorium, AuditoriumId> auditoriumRepository;

        public EndShowtimeInMovieCommandHandler(IRepository<Auditorium, AuditoriumId> auditoriumRepository)
        {
            this.auditoriumRepository = auditoriumRepository ?? throw new ArgumentNullException(nameof(auditoriumRepository));
        }

        public async Task<Result> Handle(EndShowtimeInMovieCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(new AuditoriumId(request.MovieId), cancellationToken)
                                    ?? throw new NotFoundException(nameof(Auditorium), request.MovieId.ToString());

            existingAuditorium.EndShowtime(new ShowtimeId(request.ShowtimeId));

            await this.auditoriumRepository.UpdateAsync(existingAuditorium, cancellationToken);

            return Result.NoContent();
        }
    }
}