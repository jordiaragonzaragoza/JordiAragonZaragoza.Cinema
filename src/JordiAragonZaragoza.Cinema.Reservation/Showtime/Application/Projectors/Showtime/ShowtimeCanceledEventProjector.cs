namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Projectors.Showtime
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Application.Handlers;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class ShowtimeCanceledEventProjector : BaseEventHandler<ShowtimeCanceledEvent>
    {
        private readonly IRepository<ShowtimeReadModel, Guid> showtimeReadModelRepository;

        public ShowtimeCanceledEventProjector(
            IRepository<ShowtimeReadModel, Guid> showtimeReadModelRepository)
        {
            this.showtimeReadModelRepository = Guard.Against.Null(showtimeReadModelRepository, nameof(showtimeReadModelRepository));
        }

        public override async Task HandleAsync(ShowtimeCanceledEvent @event, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(@event);

            var readModel = await this.showtimeReadModelRepository.GetByIdAsync(@event.AggregateId, cancellationToken);
            if (readModel is null)
            {
                throw new NotFoundException(nameof(ShowtimeReadModel), @event.AggregateId.ToString());
            }

            await this.showtimeReadModelRepository.DeleteAsync(readModel, cancellationToken);
        }
    }
}