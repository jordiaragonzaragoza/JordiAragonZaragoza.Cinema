namespace JordiAragon.Cinema.Reservation.Showtime.Application.CommandHandlers.PurchaseTicket
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public class PurchaseTicketCommandHandler : BaseCommandHandler<PurchaseTicketCommand>
    {
        private readonly IRepository<Showtime, ShowtimeId> showtimeRepository;

        public PurchaseTicketCommandHandler(
            IRepository<Showtime, ShowtimeId> showtimeRepository)
        {
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
        }

        public override async Task<Result> Handle(PurchaseTicketCommand request, CancellationToken cancellationToken)
        {
            var existingShowtime = await this.showtimeRepository.GetByIdAsync(ShowtimeId.Create(request.ShowtimeId), cancellationToken);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)}: {request.ShowtimeId} not found.");
            }

            existingShowtime.PurchaseTicket(TicketId.Create(request.TicketId));

            await this.showtimeRepository.UpdateAsync(existingShowtime, cancellationToken);

            return Result.Success();
        }
    }
}