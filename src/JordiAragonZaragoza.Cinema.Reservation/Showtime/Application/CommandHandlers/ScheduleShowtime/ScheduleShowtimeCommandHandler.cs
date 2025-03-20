﻿namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.ScheduleShowtime
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Commands;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class ScheduleShowtimeCommandHandler : BaseCommandHandler<ScheduleShowtimeCommand>
    {
        private readonly IReadRepository<Auditorium, AuditoriumId> auditoriumRepository;
        private readonly IReadRepository<Movie, MovieId> movieRepository;
        private readonly IRepository<Showtime, ShowtimeId> showtimeRepository;

        public ScheduleShowtimeCommandHandler(
            IReadRepository<Auditorium, AuditoriumId> auditoriumRepository,
            IReadRepository<Movie, MovieId> movieRepository,
            IRepository<Showtime, ShowtimeId> showtimeRepository)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
        }

        public override async Task<Result> Handle(ScheduleShowtimeCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(new AuditoriumId(request.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {request.AuditoriumId} not found.");
            }

            var existingMovie = await this.movieRepository.GetByIdAsync(new MovieId(request.MovieId), cancellationToken);
            if (existingMovie is null)
            {
                return Result.NotFound($"{nameof(Movie)}: {request.MovieId} not found.");
            }

            // TODO: This is temporal. A scheduler manager with business rules is required.
            var newShowtime = Showtime.Schedule(
                new ShowtimeId(request.ShowtimeId),
                new MovieId(existingMovie.Id),
                SessionDate.Create(request.SessionDateOnUtc),
                new AuditoriumId(existingAuditorium.Id));

            await this.showtimeRepository.AddAsync(newShowtime, cancellationToken);

            return Result.NoContent();
        }
    }
}