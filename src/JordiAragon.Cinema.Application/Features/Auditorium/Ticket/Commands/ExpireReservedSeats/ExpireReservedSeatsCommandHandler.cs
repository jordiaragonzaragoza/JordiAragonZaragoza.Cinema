namespace JordiAragon.Cinema.Application.Features.Auditorium.Ticket.Commands.ExpireReservedSeats
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

    public class ExpireReservedSeatsCommandHandler : ICommandHandler<ExpireReservedSeatsCommand>
    {
        private readonly IRepository<Auditorium> auditoriumRepository;

        public ExpireReservedSeatsCommandHandler(
            IRepository<Auditorium> auditoriumRepository)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
        }

        public async Task<Result> Handle(ExpireReservedSeatsCommand request, CancellationToken cancellationToken)
        {
            var existingAuditorium = await this.auditoriumRepository.FirstOrDefaultAsync(new AuditoriumWithTicketSeatByTicketIdSpec(TicketId.Create(request.TicketId)), cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Ticket)}: {request.TicketId} not found.");
            }

            var existingShowtime = existingAuditorium.Showtimes.FirstOrDefault(showtime => showtime.Tickets.Any(ticket => ticket.Id == TicketId.Create(request.TicketId)));
            if (existingShowtime is null)
            {
                return Result.NotFound($"{nameof(Showtime)} not found for {nameof(Ticket)}: {request.TicketId}.");
            }

            existingShowtime.ExpireReservedSeats(TicketId.Create(request.TicketId));

            await this.auditoriumRepository.UpdateAsync(existingAuditorium, cancellationToken);

            return Result.Success();
        }
    }
}