namespace JordiAragon.Cinema.Reservation.Showtime.Application.Commands.DeleteShowtime
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Events;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Specifications;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class DeleteShowtimeCommandHandler : BaseCommandHandler<DeleteShowtimeCommand>
    {
        private readonly IRepository<Showtime> showtimeRepository;

        public DeleteShowtimeCommandHandler(
            IRepository<Showtime> showtimeRepository)
        {
            this.showtimeRepository = Guard.Against.Null(showtimeRepository, nameof(showtimeRepository));
        }

        public override async Task<Result> Handle(DeleteShowtimeCommand request, CancellationToken cancellationToken)
        {
            var existingShowtime = await this.showtimeRepository.FirstOrDefaultAsync(new ShowtimeByIdSpec(ShowtimeId.Create(request.ShowtimeId)), cancellationToken);
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)}: {request.ShowtimeId} not found.");
            }

            await this.showtimeRepository.DeleteAsync(existingShowtime, cancellationToken);

            this.RegisterApplicationEvent(new ShowtimeDeletedEvent(existingShowtime.Id.Value, existingShowtime.AuditoriumId.Value, existingShowtime.MovieId.Value));

            return Result.Success();
        }
    }
}