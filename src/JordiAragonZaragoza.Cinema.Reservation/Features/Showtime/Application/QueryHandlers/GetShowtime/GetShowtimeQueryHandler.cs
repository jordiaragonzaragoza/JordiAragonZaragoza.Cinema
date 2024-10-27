namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtime
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

    public sealed class GetShowtimeQueryHandler : IQueryHandler<GetShowtimeQuery, ShowtimeReadModel>
    {
        private readonly IReadRepository<ShowtimeReadModel, Guid> showtimeReadModelRepository;

        public GetShowtimeQueryHandler(IReadRepository<ShowtimeReadModel, Guid> showtimeReadModelRepository)
        {
            this.showtimeReadModelRepository = Guard.Against.Null(showtimeReadModelRepository, nameof(showtimeReadModelRepository));
        }

        public async Task<Result<ShowtimeReadModel>> Handle(GetShowtimeQuery request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var result = await this.showtimeReadModelRepository.GetByIdAsync(request.ShowtimeId, cancellationToken);
            if (result is null)
            {
                return Result.NotFound("Showtime not found.");
            }

            return Result.Success(result);
        }
    }
}