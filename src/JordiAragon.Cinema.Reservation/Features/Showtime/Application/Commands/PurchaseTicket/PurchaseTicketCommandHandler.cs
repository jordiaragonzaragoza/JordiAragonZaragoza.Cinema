namespace JordiAragon.Cinema.Reservation.Showtime.Application.Commands.PurchaseTicket
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

    public class PurchaseTicketCommandHandler : BaseCommandHandler<PurchaseTicketCommand, TicketOutputDto>
    {
        private readonly IRepository<Showtime, ShowtimeId> showtimeRepository;
        private readonly IRepository<Movie, MovieId> movieRepository;
        private readonly IRepository<Auditorium, AuditoriumId> auditoriumRepository;
        private readonly IMapper mapper;

        public PurchaseTicketCommandHandler(
            IRepository<Auditorium, AuditoriumId> auditoriumReadRepository,
            IRepository<Movie, MovieId> movieReadRepository,
            IRepository<Showtime, ShowtimeId> showtimeRepository,
            IMapper mapper)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumReadRepository, nameof(auditoriumReadRepository));
            this.movieRepository = Guard.Against.Null(movieReadRepository, nameof(movieReadRepository));
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override async Task<Result<TicketOutputDto>> Handle(PurchaseTicketCommand request, CancellationToken cancellationToken)
        {
            var existingShowtime = await this.showtimeRepository.GetByIdAsync(ShowtimeId.Create(request.ShowtimeId), cancellationToken);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)}: {request.ShowtimeId} not found.");
            }

            var existingTicket = existingShowtime.PurchaseTicket(TicketId.Create(request.TicketId));

            await this.showtimeRepository.UpdateAsync(existingShowtime, cancellationToken);

            // TODO: Prepare OutputDto: Replace with correct projections on EventSourcing.
            var existingmovie = await this.movieRepository.GetByIdAsync(existingShowtime.MovieId, cancellationToken);
            if (existingmovie is null)
            {
                return Result.NotFound($"{nameof(Movie)}: {existingShowtime.MovieId} not found.");
            }

            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(existingShowtime.AuditoriumId, cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {existingShowtime.AuditoriumId} not found.");
            }

            var seats = existingAuditorium.Seats.Where(seat => existingTicket.Seats.Contains(seat.Id));

            var ticketOutputDto = new TicketOutputDto(
                existingTicket.Id,
                existingShowtime.SessionDateOnUtc,
                existingAuditorium.Id,
                existingmovie.Title,
                this.mapper.Map<IEnumerable<SeatOutputDto>>(seats),
                existingTicket.IsPurchased);

            return Result.Success(ticketOutputDto);
        }
    }
}