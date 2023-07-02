namespace JordiAragon.Cinema.Application.Features.Showtime.Commands.PurchaseSeats
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Application.Contracts.Features.Showtime.Commands;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate.Specifications;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class PurchaseSeatsCommandHandler : ICommandHandler<PurchaseSeatsCommand>
    {
        private readonly IRepository<Showtime> showtimeRepository;

        public PurchaseSeatsCommandHandler(
            IRepository<Showtime> showtimeRepository)
        {
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
        }

        public async Task<Result> Handle(PurchaseSeatsCommand request, CancellationToken cancellationToken)
        {
            var specification = new ShowtimeByTicketIdSpec(TicketId.Create(request.TicketId));
            var existingShowtime = await this.showtimeRepository.FirstOrDefaultAsync(specification, cancellationToken);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Ticket)}: {request.TicketId} not found.");
            }

            existingShowtime.PurchaseSeats(TicketId.Create(request.TicketId));

            await this.showtimeRepository.UpdateAsync(existingShowtime, cancellationToken);

            return Result.Success();
        }
    }
}