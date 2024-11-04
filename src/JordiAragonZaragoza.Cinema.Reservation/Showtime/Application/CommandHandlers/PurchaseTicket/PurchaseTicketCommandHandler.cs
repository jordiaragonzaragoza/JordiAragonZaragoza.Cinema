namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.PurchaseTicket
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Commands;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class PurchaseTicketCommandHandler : BaseCommandHandler<PurchaseTicketCommand>
    {
        private readonly IRepository<Showtime, ShowtimeId> showtimeRepository;

        public PurchaseTicketCommandHandler(
            IRepository<Showtime, ShowtimeId> showtimeRepository)
        {
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
        }

        public override async Task<Result> Handle(PurchaseTicketCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var existingShowtime = await this.showtimeRepository.GetByIdAsync(new ShowtimeId(request.ShowtimeId), cancellationToken);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)}: {request.ShowtimeId} not found.");
            }

            existingShowtime.PurchaseTicket(new TicketId(request.TicketId));

            await this.showtimeRepository.UpdateAsync(existingShowtime, cancellationToken);

            return Result.Success();
        }
    }
}