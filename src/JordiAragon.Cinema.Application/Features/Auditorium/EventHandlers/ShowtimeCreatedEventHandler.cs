namespace JordiAragon.Cinema.Application.Features.Auditorium.EventHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Specifications;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate.Events;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class ShowtimeCreatedEventHandler : IDomainEventHandler<ShowtimeCreatedEvent>
    {
        private readonly IRepository<Auditorium> auditoriumRepository;

        public ShowtimeCreatedEventHandler(
            IRepository<Auditorium> auditoriumRepository)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
        }

        public async Task Handle(ShowtimeCreatedEvent @event, CancellationToken cancellationToken)
        {
            var existingAuditorium = await this.auditoriumRepository.FirstOrDefaultAsync(new AuditoriumByIdSpec(@event.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                return; // TODO: Complete.
            }

            existingAuditorium.AddShowtime(@event.ShowtimeId);

            await this.auditoriumRepository.UpdateAsync(existingAuditorium, cancellationToken);
        }
    }
}