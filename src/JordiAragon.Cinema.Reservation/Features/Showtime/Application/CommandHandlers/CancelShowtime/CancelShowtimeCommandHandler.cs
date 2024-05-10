namespace JordiAragon.Cinema.Reservation.Showtime.Application.CommandHandlers.CancelShowtime
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Events;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Contracts.Repositories;

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
            var existingShowtime = await this.showtimeRepository.GetByIdAsync(ShowtimeId.Create(request.ShowtimeId), cancellationToken);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)}: {request.ShowtimeId} not found.");
            }

            await this.showtimeRepository.DeleteAsync(existingShowtime, cancellationToken);

            this.RegisterApplicationEvent(new ShowtimeCanceledEvent(existingShowtime.Id, existingShowtime.AuditoriumId, existingShowtime.MovieId));

            return Result.Success();
        }
    }
}