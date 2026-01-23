namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Projectors.Auditorium
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class AuditoriumRemovedEventProjector : BaseEventHandler<AuditoriumRemovedEvent>
    {
        private readonly IRepository<AuditoriumReadModel, Guid> auditoriumReadModelRepository;

        public AuditoriumRemovedEventProjector(
            IRepository<AuditoriumReadModel, Guid> auditoriumReadModelRepository)
        {
            this.auditoriumReadModelRepository = auditoriumReadModelRepository ?? throw new ArgumentNullException(nameof(auditoriumReadModelRepository));
        }

        public override async Task HandleAsync(AuditoriumRemovedEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event, nameof(@event));

            var readModel = await this.auditoriumReadModelRepository.GetByIdAsync(@event.AggregateId, cancellationToken);
            if (readModel is null)
            {
                throw new NotFoundException(nameof(AuditoriumReadModel), @event.AggregateId.ToString());
            }

            await this.auditoriumReadModelRepository.DeleteAsync(readModel, cancellationToken);
        }
    }
}