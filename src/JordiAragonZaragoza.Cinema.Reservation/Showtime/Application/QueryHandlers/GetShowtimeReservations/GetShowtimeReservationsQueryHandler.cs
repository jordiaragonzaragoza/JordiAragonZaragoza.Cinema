namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtimeReservations
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed class GetShowtimeReservationsQueryHandler : IQueryHandler<GetShowtimeReservationsQuery, PaginatedCollectionOutputDto<ReservationReadModel>>
    {
        private readonly IPaginatedSpecificationReadRepository<ReservationReadModel> reservationReadModelRepository;

        public GetShowtimeReservationsQueryHandler(IPaginatedSpecificationReadRepository<ReservationReadModel> showtimeReadModelRepository)
        {
            this.reservationReadModelRepository = Guard.Against.Null(showtimeReadModelRepository, nameof(showtimeReadModelRepository));
        }

        public async Task<Result<PaginatedCollectionOutputDto<ReservationReadModel>>> Handle(GetShowtimeReservationsQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetShowtimeReservationsSpec(request);
            var result = await this.reservationReadModelRepository.PaginatedListAsync(specification, cancellationToken);
            if (!result.Items.Any())
            {
                return Result.NotFound("Reservation/s not found.");
            }

            return Result.Success(result);
        }
    }
}