namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.CancelShowtime
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Commands;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class CancelShowtimeCommandHandler : BaseCommandHandler<CancelShowtimeCommand>
    {
        private readonly IRepository<Showtime, ShowtimeId> showtimeRepository;

        public CancelShowtimeCommandHandler(
            IRepository<Showtime, ShowtimeId> showtimeRepository)
        {
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
        }

        public override async Task<Result> Handle(CancelShowtimeCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var existingShowtime = await this.showtimeRepository.GetByIdAsync(new ShowtimeId(request.ShowtimeId), cancellationToken);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)}: {request.ShowtimeId} not found.");
            }

            existingShowtime.Cancel();
            await this.showtimeRepository.DeleteAsync(existingShowtime, cancellationToken);

            return Result.Success();
        }
    }
}