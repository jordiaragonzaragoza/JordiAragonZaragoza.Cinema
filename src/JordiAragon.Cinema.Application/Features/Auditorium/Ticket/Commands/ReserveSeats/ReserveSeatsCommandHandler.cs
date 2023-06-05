namespace JordiAragon.Cinema.Application.Features.Auditorium.Ticket.Commands.ReserveSeats
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Seat.Queries;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Ticket.Commands;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Specifications;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using Volo.Abp.Guids;

    public class ReserveSeatsCommandHandler : ICommandHandler<ReserveSeatsCommand, TicketOutputDto>
    {
        private readonly IRepository<Auditorium> auditoriumRepository;
        private readonly IGuidGenerator guidGenerator;
        private readonly IMapper mapper;
        private readonly IDateTime dateTime;
        private readonly IReadRepository<Movie> movieReadRepository;

        public ReserveSeatsCommandHandler(
            IRepository<Auditorium> auditoriumRepository,
            IReadRepository<Movie> movieReadRepository,
            IMapper mapper,
            IGuidGenerator guidGenerator,
            IDateTime dateTime)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.movieReadRepository = Guard.Against.Null(movieReadRepository, nameof(movieReadRepository));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
            this.guidGenerator = Guard.Against.Null(guidGenerator, nameof(guidGenerator));
            this.dateTime = Guard.Against.Null(dateTime, nameof(dateTime));
        }

        public async Task<Result<TicketOutputDto>> Handle(ReserveSeatsCommand request, CancellationToken cancellationToken)
        {
            var specification = new AuditoriumExtendedByIdShowtimeIdSpec(AuditoriumId.Create(request.AuditoriumId), ShowtimeId.Create(request.ShowtimeId));
            var existingAuditorium = await this.auditoriumRepository.FirstOrDefaultAsync(specification, cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {request.AuditoriumId} not found.");
            }

            var showtimeId = ShowtimeId.Create(request.ShowtimeId);
            var existingShowtime = existingAuditorium.Showtimes.FirstOrDefault(showtime => showtime.Id == showtimeId);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)}: {request.ShowtimeId} not found.");
            }

            // Make the reserve.
            var desiredSeatsIds = this.mapper.Map<IEnumerable<SeatId>>(request.SeatsIds);
            var newticket = existingShowtime.ReserveSeats(desiredSeatsIds, TicketId.Create(this.guidGenerator.Create()), this.dateTime.UtcNow);

            // Prepare OutputDto.
            var existingmovie = await this.movieReadRepository.GetByIdAsync(existingShowtime.MovieId, cancellationToken);
            if (existingmovie is null)
            {
                return Result.NotFound($"{nameof(Movie)}: {existingShowtime.MovieId} not found.");
            }

            var seats = existingAuditorium.Seats.Where(seat => desiredSeatsIds.Contains(seat.Id));

            var ticketOutputDto = new TicketOutputDto(
                newticket.Id.Value,
                existingShowtime.SessionDateOnUtc,
                existingShowtime.Auditorium.Id.Value,
                existingmovie.Title,
                this.mapper.Map<IEnumerable<SeatOutputDto>>(seats));

            await this.auditoriumRepository.UpdateAsync(existingAuditorium, cancellationToken);

            return Result.Success(ticketOutputDto);
        }
    }
}