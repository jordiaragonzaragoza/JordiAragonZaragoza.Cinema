namespace JordiAragon.Cinemas.Ticketing.Application.Showtime.Commands.ExpireReservedSeats
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Showtime.Commands;
    using JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate;
    using JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate.Specifications;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class ExpireReservedSeatsCommandHandler : BaseCommandHandler<ExpireReservedSeatsCommand>
    {
        private readonly IRepository<Showtime> showtimeRepository;

        public ExpireReservedSeatsCommandHandler(
            IRepository<Showtime> showRepository)
        {
            this.showtimeRepository = Guard.Against.Null(showRepository, nameof(showRepository));
        }

        public override async Task<Result> Handle(ExpireReservedSeatsCommand request, CancellationToken cancellationToken)
        {
            var existingShowtime = await this.showtimeRepository.FirstOrDefaultAsync(new ShowtimeByIdSpec(ShowtimeId.Create(request.ShowtimeId)), cancellationToken);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)}: {request.ShowtimeId} not found.");
            }

            existingShowtime.ExpireReservedSeats(TicketId.Create(request.TicketId));

            await this.showtimeRepository.UpdateAsync(existingShowtime, cancellationToken);

            return Result.Success();
        }
    }
}