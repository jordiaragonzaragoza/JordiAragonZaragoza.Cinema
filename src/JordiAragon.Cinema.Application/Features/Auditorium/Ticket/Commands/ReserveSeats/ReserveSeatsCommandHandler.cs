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
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using Volo.Abp.Guids;

    public class ReserveSeatsCommandHandler : ICommandHandler<ReserveSeatsCommand, TicketOutputDto>
    {
        private readonly IReadRepository<Auditorium> auditoriumRepository;
        private readonly IGuidGenerator guidGenerator;
        private readonly IMapper mapper;
        private readonly IDateTime dateTime;
        private readonly IReadRepository<Movie> movieReadRepository;
        private readonly IRepository<Showtime> showtimeRepository;

        public ReserveSeatsCommandHandler(
            IReadRepository<Auditorium> auditoriumRepository,
            IReadRepository<Movie> movieReadRepository,
            IRepository<Showtime> showtimeRepository,
            IMapper mapper,
            IGuidGenerator guidGenerator,
            IDateTime dateTime)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.movieReadRepository = Guard.Against.Null(movieReadRepository, nameof(movieReadRepository));
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
            this.guidGenerator = Guard.Against.Null(guidGenerator, nameof(guidGenerator));
            this.dateTime = Guard.Against.Null(dateTime, nameof(dateTime));
        }

        public async Task<Result<TicketOutputDto>> Handle(ReserveSeatsCommand request, CancellationToken cancellationToken)
        {
            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(AuditoriumId.Create(request.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {request.AuditoriumId} not found.");
            }

            var existingShowtime = await this.showtimeRepository.GetByIdAsync(ShowtimeId.Create(request.ShowtimeId), cancellationToken);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)}: {request.ShowtimeId} not found.");
            }

            // Make the reserve.
            var desiredSeatsIds = this.mapper.Map<IEnumerable<SeatId>>(request.SeatsIds);

            var newticket = ShowtimeManager.ReserveSeats(
                existingAuditorium,
                existingShowtime,
                desiredSeatsIds,
                TicketId.Create(this.guidGenerator.Create()),
                this.dateTime.UtcNow);

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
                existingAuditorium.Id.Value,
                existingmovie.Title,
                this.mapper.Map<IEnumerable<SeatOutputDto>>(seats));

            await this.showtimeRepository.UpdateAsync(existingShowtime, cancellationToken);

            return Result.Success(ticketOutputDto);
        }
    }
}