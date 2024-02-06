namespace JordiAragon.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtimes
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

    public class GetShowtimesQueryHandler : IQueryHandler<GetShowtimesQuery, IEnumerable<ShowtimeReadModel>>
    {
        private readonly ISpecificationReadRepository<ShowtimeReadModel, Guid> showtimeReadModelRepository;

        public GetShowtimesQueryHandler(ISpecificationReadRepository<ShowtimeReadModel, Guid> showtimeReadModelRepository)
        {
            this.showtimeReadModelRepository = Guard.Against.Null(showtimeReadModelRepository, nameof(showtimeReadModelRepository));
        }

        public async Task<Result<IEnumerable<ShowtimeReadModel>>> Handle(GetShowtimesQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetShowtimesSpec(request);
            var showtimes = await this.showtimeReadModelRepository.ListAsync(specification, cancellationToken);
            if (!showtimes.Any())
            {
                return Result.NotFound($"{nameof(Showtime)}/s not found.");
            }

            return Result.Success(showtimes.AsEnumerable());
        }
    }
}