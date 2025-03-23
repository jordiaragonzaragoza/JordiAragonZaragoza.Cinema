namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserReservations
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed class GetUserReservationsQueryHandler : IQueryHandler<GetUserReservationsQuery, PaginatedCollectionOutputDto<ReservationReadModel>>
    {
        private readonly IPaginatedSpecificationReadRepository<ReservationReadModel> reservationReadModelRepository;

        public GetUserReservationsQueryHandler(
            IPaginatedSpecificationReadRepository<ReservationReadModel> reservationReadModelRepository)
        {
            this.reservationReadModelRepository = reservationReadModelRepository;
        }

        public async Task<Result<PaginatedCollectionOutputDto<ReservationReadModel>>> Handle(GetUserReservationsQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetUserReservationsSpecification(request);
            var result = await this.reservationReadModelRepository.PaginatedListAsync(specification, cancellationToken);
            if (!result.Items.Any())
            {
                return Result.NotFound("Reservation/s not found.");
            }

            return Result.Success(result);
        }
    }
}