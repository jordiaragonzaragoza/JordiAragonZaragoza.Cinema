namespace JordiAragon.Cinema.Application.Features.Auditorium.Seat.Queries.GetAvailableSeats
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Seat.Queries;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Specifications;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class GetAvailableSeatsQueryHandler : IQueryHandler<GetAvailableSeatsQuery, IEnumerable<SeatOutputDto>>
    {
        private readonly IReadRepository<Auditorium> auditoriumRepository;
        private readonly IMapper mapper;

        public GetAvailableSeatsQueryHandler(
            IReadRepository<Auditorium> auditoriumRepository,
            IMapper mapper)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        // TODO: Oportunity to introduce Dapper.
        public async Task<Result<IEnumerable<SeatOutputDto>>> Handle(GetAvailableSeatsQuery request, CancellationToken cancellationToken)
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

            var availableSeats = existingShowtime.AvailableSeats();

            return Result.Success(this.mapper.Map<IEnumerable<SeatOutputDto>>(availableSeats));
        }
    }
}