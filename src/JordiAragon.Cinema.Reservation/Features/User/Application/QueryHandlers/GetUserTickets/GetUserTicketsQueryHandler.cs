﻿namespace JordiAragon.Cinema.Reservation.User.Application.QueryHandlers.GetUserTickets
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragon.SharedKernel.Application.Contracts;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed class GetUserTicketsQueryHandler : IQueryHandler<GetUserTicketsQuery, PaginatedCollectionOutputDto<TicketReadModel>>
    {
        private readonly IPaginatedSpecificationReadRepository<TicketReadModel> ticketReadModelRepository;

        public GetUserTicketsQueryHandler(
            IPaginatedSpecificationReadRepository<TicketReadModel> ticketReadModelRepository)
        {
            this.ticketReadModelRepository = ticketReadModelRepository;
        }

        public async Task<Result<PaginatedCollectionOutputDto<TicketReadModel>>> Handle(GetUserTicketsQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetUserTicketsSpecification(request);
            var result = await this.ticketReadModelRepository.PaginatedListAsync(specification, cancellationToken);
            if (!result.Items.Any())
            {
                return Result.NotFound("Ticket/s not found.");
            }

            return Result.Success(result);
        }
    }
}