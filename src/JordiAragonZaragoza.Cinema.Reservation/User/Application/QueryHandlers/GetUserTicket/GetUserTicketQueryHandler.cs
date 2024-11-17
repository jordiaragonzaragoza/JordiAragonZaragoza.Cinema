namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserTicket
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class GetUserTicketQueryHandler : IQueryHandler<GetUserTicketQuery, TicketReadModel>
    {
        private readonly ISpecificationReadRepository<TicketReadModel, Guid> ticketReadModelRepository;

        public GetUserTicketQueryHandler(
            ISpecificationReadRepository<TicketReadModel, Guid> ticketReadModelRepository)
        {
            this.ticketReadModelRepository = ticketReadModelRepository;
        }

        public async Task<Result<TicketReadModel>> Handle(GetUserTicketQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetUserTicketSpecification(request);
            var result = await this.ticketReadModelRepository.FirstOrDefaultAsync(specification, cancellationToken);
            if (result is null)
            {
                return Result.NotFound($"{nameof(TicketReadModel)} not found.");
            }

            return Result.Success(result);
        }
    }
}