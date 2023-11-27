namespace JordiAragon.Cinema.Reservation.Showtime.Application.Queries.GetAvailableSeats
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class GetAvailableSeatsQueryHandler : IQueryHandler<GetAvailableSeatsQuery, IEnumerable<SeatOutputDto>>
    {
        private readonly IReadRepository<Auditorium, AuditoriumId, Guid> auditoriumRepository;
        private readonly IReadRepository<Showtime, ShowtimeId, Guid> showtimeRepository;
        private readonly IMapper mapper;

        public GetAvailableSeatsQueryHandler(
            IReadRepository<Auditorium, AuditoriumId, Guid> auditoriumRepository,
            IReadRepository<Showtime, ShowtimeId, Guid> showtimeRepository,
            IMapper mapper)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public async Task<Result<IEnumerable<SeatOutputDto>>> Handle(GetAvailableSeatsQuery request, CancellationToken cancellationToken)
        {
            var existingShowtime = await this.showtimeRepository.GetByIdAsync(ShowtimeId.Create(request.ShowtimeId), cancellationToken);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)}: {request.ShowtimeId} not found.");
            }

            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(existingShowtime.AuditoriumId, cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {existingShowtime.AuditoriumId} not found.");
            }

            var availableSeats = ShowtimeManager.AvailableSeats(existingAuditorium, existingShowtime);

            return Result.Success(this.mapper.Map<IEnumerable<SeatOutputDto>>(availableSeats));
        }
    }
}