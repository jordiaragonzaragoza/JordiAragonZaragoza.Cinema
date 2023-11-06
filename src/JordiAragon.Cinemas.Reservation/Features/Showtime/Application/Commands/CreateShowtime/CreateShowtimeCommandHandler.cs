﻿namespace JordiAragon.Cinemas.Reservation.Showtime.Application.Commands.CreateShowtime
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinemas.Reservation.Auditorium.Domain;
    using JordiAragon.Cinemas.Reservation.Auditorium.Domain.Specifications;
    using JordiAragon.Cinemas.Reservation.Movie.Domain;
    using JordiAragon.Cinemas.Reservation.Movie.Domain.Specifications;
    using JordiAragon.Cinemas.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinemas.Reservation.Showtime.Domain;
    using JordiAragon.Cinemas.Reservation.Showtime.Domain.Specifications;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using Volo.Abp.Guids;

    public class CreateShowtimeCommandHandler : BaseCommandHandler<CreateShowtimeCommand, Guid>
    {
        private readonly IReadRepository<Movie> movieRepository;
        private readonly IReadRepository<Auditorium> auditoriumRepository;
        private readonly IRepository<Showtime> showtimeRepository;
        private readonly IGuidGenerator guidGenerator;

        public CreateShowtimeCommandHandler(
            IReadRepository<Auditorium> auditoriumRepository,
            IReadRepository<Movie> movieRepository,
            IRepository<Showtime> showtimeRepository,
            IGuidGenerator guidGenerator)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.movieRepository = Guard.Against.Null(movieRepository, nameof(movieRepository));
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
            this.guidGenerator = Guard.Against.Null(guidGenerator, nameof(guidGenerator));
        }

        public override async Task<Result<Guid>> Handle(CreateShowtimeCommand request, CancellationToken cancellationToken)
        {
            var existingShowtime = await this.showtimeRepository.FirstOrDefaultAsync(new ShowtimeByMovieIdSessionDateSpec(MovieId.Create(request.MovieId), request.SessionDateOnUtc), cancellationToken);
            if (existingShowtime is not null)
            {
                return Result.Invalid(new List<ValidationError>() { new ValidationError() { ErrorMessage = $"{nameof(Showtime)} already exists for this {nameof(Movie)}: {request.MovieId}" } });
            }

            var existingAuditorium = await this.auditoriumRepository.FirstOrDefaultAsync(new AuditoriumByIdSpec(AuditoriumId.Create(request.AuditoriumId)), cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {request.AuditoriumId} not found.");
            }

            var existingMovie = await this.movieRepository.FirstOrDefaultAsync(new MovieByIdSpec(MovieId.Create(request.MovieId)), cancellationToken);
            if (existingMovie is null)
            {
                return Result.NotFound($"{nameof(Movie)}: {request.MovieId} not found.");
            }

            var newShowtime = Showtime.Create(
                ShowtimeId.Create(this.guidGenerator.Create()),
                MovieId.Create(existingMovie.Id.Value),
                request.SessionDateOnUtc,
                AuditoriumId.Create(existingAuditorium.Id.Value));

            await this.showtimeRepository.AddAsync(newShowtime, cancellationToken);

            return Result.Success(newShowtime.Id.Value);
        }
    }
}