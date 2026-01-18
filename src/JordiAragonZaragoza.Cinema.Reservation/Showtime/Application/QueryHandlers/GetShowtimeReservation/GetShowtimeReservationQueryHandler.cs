namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtimeReservation
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class GetShowtimeReservationQueryHandler : IQueryHandler<GetShowtimeReservationQuery, ReservationReadModel>
    {
        private readonly IReadRepository<ReservationReadModel, Guid> repository;

        public GetShowtimeReservationQueryHandler(IReadRepository<ReservationReadModel, Guid> repository)
        {
            this.repository = Guard.Against.Null(repository, nameof(repository));
        }

        public async Task<Result<ReservationReadModel>> Handle(GetShowtimeReservationQuery request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var result = await this.repository.GetByIdAsync(request.ReservationId, cancellationToken);
            if (result is null)
            {
                return Result.NotFound("Reservation not found.");
            }

            return Result.Success(result);
        }
    }
}