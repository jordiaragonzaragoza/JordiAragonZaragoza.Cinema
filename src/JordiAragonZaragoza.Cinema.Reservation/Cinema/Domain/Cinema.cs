namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;

    public sealed class Cinema : BaseEventSourcedAggregateRoot<CinemaId, Guid>
    {
        // Required by EF.
        private Cinema()
        {
        }

        public static Cinema Create(
            CinemaId id)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            var cinema = new Cinema();

            cinema.Apply(new CinemaCreatedEvent(id));

            return cinema;
        }

        public void Remove()
            => this.Apply(new CinemaRemovedEvent(this.Id));

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case CinemaCreatedEvent @event:
                    this.Applier(@event);
                    break;

                case CinemaRemovedEvent:
                    break;

                default:
                    throw new EventCannotBeAppliedToAggregateException<Cinema, CinemaId>(this, domainEvent);
            }
        }

        protected override void EnsureValidState()
        {
            // Not required validation post apply events. This is a deterministic aggregate.
            // All the validations are done on public methods.
        }

        private void Applier(CinemaCreatedEvent @event)
        {
            this.Id = new CinemaId(@event.AggregateId);
        }
    }
}