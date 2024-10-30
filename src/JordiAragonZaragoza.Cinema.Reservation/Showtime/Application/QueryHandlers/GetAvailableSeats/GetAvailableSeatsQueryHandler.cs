namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetAvailableSeats
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class GetAvailableSeatsQueryHandler : IQueryHandler<GetAvailableSeatsQuery, IEnumerable<AvailableSeatReadModel>>
    {
        private readonly ISpecificationReadRepository<AvailableSeatReadModel, Guid> readListRepository;

        public GetAvailableSeatsQueryHandler(ISpecificationReadRepository<AvailableSeatReadModel, Guid> readListRepository)
        {
            this.readListRepository = Guard.Against.Null(readListRepository, nameof(readListRepository));
        }

        public async Task<Result<IEnumerable<AvailableSeatReadModel>>> Handle(GetAvailableSeatsQuery request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var specification = new GetAvailableSeatsByShowtimeIdSpec(request.ShowtimeId);
            var result = await this.readListRepository.ListAsync(specification, cancellationToken);
            if (result.Count == 0)
            {
                return Result.NotFound($"{nameof(AvailableSeatReadModel)}/s not found.");
            }

            return Result.Success(result.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber).AsEnumerable());
        }
    }
}