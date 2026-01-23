namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.CommandHandlers.ExpireReservedSeats
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class ExpireReservedSeatsCommandHandler : ICommandHandler<ExpireReservedSeatsCommand>
    {
        private readonly IRepository<Showtime, ShowtimeId> showtimeRepository;

        public ExpireReservedSeatsCommandHandler(
            IRepository<Showtime, ShowtimeId> showRepository)
        {
            this.showtimeRepository = Guard.Against.Null(showRepository, nameof(showRepository));
        }

        public async Task<Result> Handle(ExpireReservedSeatsCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var existingShowtime = await this.showtimeRepository.GetByIdAsync(new ShowtimeId(request.ShowtimeId), cancellationToken);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)}: {request.ShowtimeId} not found.");
            }

            existingShowtime.ExpireReservedSeats(new ReservationId(request.ReservationId));

            await this.showtimeRepository.UpdateAsync(existingShowtime, cancellationToken);

            return Result.NoContent();
        }
    }
}