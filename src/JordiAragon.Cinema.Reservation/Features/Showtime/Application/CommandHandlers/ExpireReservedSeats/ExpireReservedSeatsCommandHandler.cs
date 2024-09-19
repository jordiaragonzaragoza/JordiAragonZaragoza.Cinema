namespace JordiAragon.Cinema.Reservation.Showtime.Application.CommandHandlers.ExpireReservedSeats
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public sealed class ExpireReservedSeatsCommandHandler : BaseCommandHandler<ExpireReservedSeatsCommand>
    {
        private readonly IRepository<Showtime, ShowtimeId> showtimeRepository;

        public ExpireReservedSeatsCommandHandler(
            IRepository<Showtime, ShowtimeId> showRepository)
        {
            this.showtimeRepository = Guard.Against.Null(showRepository, nameof(showRepository));
        }

        public override async Task<Result> Handle(ExpireReservedSeatsCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var existingShowtime = await this.showtimeRepository.GetByIdAsync(ShowtimeId.Create(request.ShowtimeId), cancellationToken);
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