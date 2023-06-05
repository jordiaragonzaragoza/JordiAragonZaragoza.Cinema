namespace JordiAragon.Cinema.Application.Features.Auditorium.Ticket.Commands.PurchaseSeats
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Ticket.Commands;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Specifications;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class PurchaseSeatsCommandHandler : ICommandHandler<PurchaseSeatsCommand>
    {
        private readonly IRepository<Auditorium> auditoriumRepository;

        public PurchaseSeatsCommandHandler(
            IRepository<Auditorium> auditoriumRepository)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
        }

        public async Task<Result> Handle(PurchaseSeatsCommand request, CancellationToken cancellationToken)
        {
            var specification = new AuditoriumWithReservedSeatsByIdShowtimeIdTicketIdSpec(AuditoriumId.Create(request.AuditoriumId), ShowtimeId.Create(request.ShowtimeId), TicketId.Create(request.TicketId));
            var existingAuditorium = await this.auditoriumRepository.FirstOrDefaultAsync(specification, cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {request.AuditoriumId} not found.");
            }

            var showtimeId = ShowtimeId.Create(request.ShowtimeId);

            var showtime = existingAuditorium.Showtimes.FirstOrDefault(showtime => showtime.Id == showtimeId);
            if (showtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)}: {request.ShowtimeId} not found.");
            }

            showtime.PurchaseSeats(TicketId.Create(request.TicketId));

            await this.auditoriumRepository.UpdateAsync(existingAuditorium, cancellationToken);

            return Result.Success();
        }
    }
}