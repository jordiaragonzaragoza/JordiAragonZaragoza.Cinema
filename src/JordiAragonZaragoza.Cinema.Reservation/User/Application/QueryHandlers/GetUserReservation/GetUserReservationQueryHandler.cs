namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserReservation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class GetUserReservationQueryHandler : IQueryHandler<GetUserReservationQuery, ReservationReadModel>
    {
        private readonly ISpecificationReadRepository<ReservationReadModel, Guid> reservationReadModelRepository;

        public GetUserReservationQueryHandler(
            ISpecificationReadRepository<ReservationReadModel, Guid> reservationReadModelRepository)
        {
            this.reservationReadModelRepository = reservationReadModelRepository;
        }

        public async Task<Result<ReservationReadModel>> Handle(GetUserReservationQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetUserReservationSpecification(request);
            var result = await this.reservationReadModelRepository.FirstOrDefaultAsync(specification, cancellationToken);
            if (result is null)
            {
                return Result.NotFound($"{nameof(ReservationReadModel)} not found.");
            }

            return Result.Success(result);
        }
    }
}