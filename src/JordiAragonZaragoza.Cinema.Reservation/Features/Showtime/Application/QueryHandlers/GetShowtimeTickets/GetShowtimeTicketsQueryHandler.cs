namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtimeTickets
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

    public sealed class GetShowtimeTicketsQueryHandler : IQueryHandler<GetShowtimeTicketsQuery, PaginatedCollectionOutputDto<TicketReadModel>>
    {
        private readonly IPaginatedSpecificationReadRepository<TicketReadModel> ticketReadModelRepository;

        public GetShowtimeTicketsQueryHandler(IPaginatedSpecificationReadRepository<TicketReadModel> showtimeReadModelRepository)
        {
            this.ticketReadModelRepository = Guard.Against.Null(showtimeReadModelRepository, nameof(showtimeReadModelRepository));
        }

        public async Task<Result<PaginatedCollectionOutputDto<TicketReadModel>>> Handle(GetShowtimeTicketsQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetShowtimeTicketsSpec(request);
            var result = await this.ticketReadModelRepository.PaginatedListAsync(specification, cancellationToken);
            if (!result.Items.Any())
            {
                return Result.NotFound("Ticket/s not found.");
            }

            return Result.Success(result);
        }
    }
}