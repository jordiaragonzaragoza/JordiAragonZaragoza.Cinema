﻿namespace JordiAragon.Cinema.Reservation.Showtime.Application.Commands.ReserveSeats
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using Volo.Abp.Guids;

    public class ReserveSeatsCommandHandler : BaseCommandHandler<ReserveSeatsCommand, TicketOutputDto>
    {
        private readonly IReadRepository<Auditorium, AuditoriumId, Guid> auditoriumReadRepository;
        private readonly IGuidGenerator guidGenerator;
        private readonly IMapper mapper;
        private readonly IDateTime dateTime;
        private readonly IReadRepository<Movie, MovieId, Guid> movieReadRepository;
        private readonly IRepository<Showtime, ShowtimeId, Guid> showtimeRepository;

        public ReserveSeatsCommandHandler(
            IReadRepository<Auditorium, AuditoriumId, Guid> auditoriumReadRepository,
            IReadRepository<Movie, MovieId, Guid> movieReadRepository,
            IRepository<Showtime, ShowtimeId, Guid> showtimeRepository,
            IMapper mapper,
            IGuidGenerator guidGenerator,
            IDateTime dateTime)
        {
            this.auditoriumReadRepository = Guard.Against.Null(auditoriumReadRepository, nameof(auditoriumReadRepository));
            this.movieReadRepository = Guard.Against.Null(movieReadRepository, nameof(movieReadRepository));
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
            this.guidGenerator = Guard.Against.Null(guidGenerator, nameof(guidGenerator));
            this.dateTime = Guard.Against.Null(dateTime, nameof(dateTime));
        }

        public override async Task<Result<TicketOutputDto>> Handle(ReserveSeatsCommand request, CancellationToken cancellationToken)
        {
            var existingShowtime = await this.showtimeRepository.GetByIdAsync(ShowtimeId.Create(request.ShowtimeId), cancellationToken);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)}: {request.ShowtimeId} not found.");
            }

            var existingAuditorium = await this.auditoriumReadRepository.GetByIdAsync(existingShowtime.AuditoriumId, cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {existingShowtime.AuditoriumId} not found.");
            }

            // Make the reserve.
            var desiredSeatsIds = this.mapper.Map<IEnumerable<SeatId>>(request.SeatsIds);

            var newticket = ShowtimeManager.ReserveSeats(
                existingAuditorium,
                existingShowtime,
                desiredSeatsIds,
                TicketId.Create(this.guidGenerator.Create()),
                this.dateTime.UtcNow);

            await this.showtimeRepository.UpdateAsync(existingShowtime, cancellationToken);

            // TODO: Prepare OutputDto: Replace with correct projections on EventSourcing.
            var existingmovie = await this.movieReadRepository.GetByIdAsync(existingShowtime.MovieId, cancellationToken);
            if (existingmovie is null)
            {
                return Result.NotFound($"{nameof(Movie)}: {existingShowtime.MovieId} not found.");
            }

            var seats = existingAuditorium.Seats.Where(seat => desiredSeatsIds.Contains(seat.Id));

            var ticketOutputDto = new TicketOutputDto(
                newticket.Id.Value,
                existingShowtime.SessionDateOnUtc,
                existingAuditorium.Id.Value,
                existingmovie.Title,
                this.mapper.Map<IEnumerable<SeatOutputDto>>(seats),
                newticket.IsPurchased);

            return Result.Success(ticketOutputDto);
        }
    }
}