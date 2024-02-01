namespace JordiAragon.Cinema.Reservation.Showtime.Application.Queries.GetAvailableSeats
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class GetAvailableSeatsQueryHandler : IQueryHandler<GetAvailableSeatsQuery, IEnumerable<SeatOutputDto>>
    {
        private readonly IReadRepository<Auditorium, AuditoriumId> auditoriumRepository;
        private readonly IReadRepository<Showtime, ShowtimeId> showtimeRepository;

        public GetAvailableSeatsQueryHandler(
            IReadRepository<Auditorium, AuditoriumId> auditoriumRepository,
            IReadRepository<Showtime, ShowtimeId> showtimeRepository)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
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

            // TODO: Prepare OutputDto: Replace with correct projections on EventSourcing.
            var seatsOutputDto = availableSeats.Select(seat
                => new SeatOutputDto(seat.Id, seat.Row, seat.SeatNumber, existingAuditorium.Id, existingAuditorium.Name));

            return Result.Success(seatsOutputDto);
        }
    }
}