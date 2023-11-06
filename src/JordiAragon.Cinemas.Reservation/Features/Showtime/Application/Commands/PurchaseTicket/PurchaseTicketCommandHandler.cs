namespace JordiAragon.Cinemas.Reservation.Showtime.Application.Commands.PurchaseTicket
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinemas.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragon.Cinemas.Reservation.Auditorium.Domain;
    using JordiAragon.Cinemas.Reservation.Auditorium.Domain.Specifications;
    using JordiAragon.Cinemas.Reservation.Movie.Domain;
    using JordiAragon.Cinemas.Reservation.Movie.Domain.Specifications;
    using JordiAragon.Cinemas.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinemas.Reservation.Showtime.Domain;
    using JordiAragon.Cinemas.Reservation.Showtime.Domain.Specifications;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class PurchaseTicketCommandHandler : BaseCommandHandler<PurchaseTicketCommand, TicketOutputDto>
    {
        private readonly IRepository<Showtime> showtimeRepository;
        private readonly IReadRepository<Movie> movieReadRepository;
        private readonly IReadRepository<Auditorium> auditoriumReadRepository;
        private readonly IMapper mapper;

        public PurchaseTicketCommandHandler(
            IReadRepository<Auditorium> auditoriumReadRepository,
            IReadRepository<Movie> movieReadRepository,
            IRepository<Showtime> showtimeRepository,
            IMapper mapper)
        {
            this.auditoriumReadRepository = Guard.Against.Null(auditoriumReadRepository, nameof(auditoriumReadRepository));
            this.movieReadRepository = Guard.Against.Null(movieReadRepository, nameof(movieReadRepository));
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public override async Task<Result<TicketOutputDto>> Handle(PurchaseTicketCommand request, CancellationToken cancellationToken)
        {
            var specification = new ShowtimeByTicketIdSpec(TicketId.Create(request.TicketId));
            var existingShowtime = await this.showtimeRepository.FirstOrDefaultAsync(specification, cancellationToken);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Ticket)}: {request.TicketId} not found.");
            }

            var existingTicket = existingShowtime.PurchaseTicket(TicketId.Create(request.TicketId));

            await this.showtimeRepository.UpdateAsync(existingShowtime, cancellationToken);

            // TODO: Prepare OutputDto: Replace with correct projections on EventSourcing.
            var existingmovie = await this.movieReadRepository.FirstOrDefaultAsync(new MovieByIdSpec(existingShowtime.MovieId), cancellationToken);
            if (existingmovie is null)
            {
                return Result.NotFound($"{nameof(Movie)}: {existingShowtime.MovieId} not found.");
            }

            var existingAuditorium = await this.auditoriumReadRepository.FirstOrDefaultAsync(new AuditoriumByIdSpec(existingShowtime.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {existingShowtime.AuditoriumId} not found.");
            }

            var seats = existingAuditorium.Seats.Where(seat => existingTicket.Seats.Contains(seat.Id));

            var ticketOutputDto = new TicketOutputDto(
                existingTicket.Id.Value,
                existingShowtime.SessionDateOnUtc,
                existingAuditorium.Id.Value,
                existingmovie.Title,
                this.mapper.Map<IEnumerable<SeatOutputDto>>(seats),
                existingTicket.IsPurchased);

            return Result.Success(ticketOutputDto);
        }
    }
}