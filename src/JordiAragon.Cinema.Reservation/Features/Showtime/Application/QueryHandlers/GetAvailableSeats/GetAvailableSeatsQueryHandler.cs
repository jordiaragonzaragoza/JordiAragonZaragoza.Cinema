namespace JordiAragon.Cinema.Reservation.Showtime.Application.QueryHandlers.GetAvailableSeats
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public class GetAvailableSeatsQueryHandler : IQueryHandler<GetAvailableSeatsQuery, IEnumerable<AvailableSeatReadModel>>
    {
        private readonly IReadListRepository<AvailableSeatReadModel, Guid> readListRepository;

        public GetAvailableSeatsQueryHandler(IReadListRepository<AvailableSeatReadModel, Guid> readListRepository)
        {
            this.readListRepository = Guard.Against.Null(readListRepository, nameof(readListRepository));
        }

        public async Task<Result<IEnumerable<AvailableSeatReadModel>>> Handle(GetAvailableSeatsQuery request, CancellationToken cancellationToken)
        {
            var result = await this.readListRepository.ListAsync(cancellationToken);
            if (!result.Any())
            {
                return Result.NotFound($"{nameof(AvailableSeatReadModel)}/s not found.");
            }

            return Result.Success(result.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber).AsEnumerable());
        }
    }
}