namespace JordiAragon.Cinemas.Reservation.Showtime.Application.Queries.GetAvailableSeats
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinemas.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragon.Cinemas.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.Cinemas.Reservation.Auditorium.Domain;
    using JordiAragon.Cinemas.Reservation.Auditorium.Domain.Specifications;
    using JordiAragon.Cinemas.Reservation.Showtime.Domain;
    using JordiAragon.Cinemas.Reservation.Showtime.Domain.Specifications;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class GetAvailableSeatsQueryHandler : IQueryHandler<GetAvailableSeatsQuery, IEnumerable<SeatOutputDto>>
    {
        private readonly IReadRepository<Auditorium> auditoriumRepository;
        private readonly IReadRepository<Showtime> showtimeRepository;
        private readonly IMapper mapper;

        public GetAvailableSeatsQueryHandler(
            IReadRepository<Auditorium> auditoriumRepository,
            IReadRepository<Showtime> showtimeRepository,
            IMapper mapper)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public async Task<Result<IEnumerable<SeatOutputDto>>> Handle(GetAvailableSeatsQuery request, CancellationToken cancellationToken)
        {
            var existingShowtime = await this.showtimeRepository.FirstOrDefaultAsync(new ShowtimeByIdSpec(ShowtimeId.Create(request.ShowtimeId)), cancellationToken);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)}: {request.ShowtimeId} not found.");
            }

            var existingAuditorium = await this.auditoriumRepository.FirstOrDefaultAsync(new AuditoriumByIdSpec(existingShowtime.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {existingShowtime.AuditoriumId} not found.");
            }

            var availableSeats = ShowtimeManager.AvailableSeats(existingAuditorium, existingShowtime);

            return Result.Success(this.mapper.Map<IEnumerable<SeatOutputDto>>(availableSeats));
        }
    }
}