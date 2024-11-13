namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;

    public sealed class User : BaseAggregateRoot<UserId, Guid>
    {
        // Required by EF.
        private User()
        {
        }

        public static User Create(
            UserId id)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));

            var user = new User();

            user.Apply(new UserCreatedEvent(id));

            return user;
        }

        public void Remove()
            => this.Apply(new UserRemovedEvent(this.Id));

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case UserCreatedEvent @event:
                    this.Applier(@event);
                    break;

                case UserRemovedEvent:
                    break;

                default:
                    throw new EventCannotBeAppliedToAggregateException<User, UserId>(this, domainEvent);
            }
        }

        protected override void EnsureValidState()
        {
            try
            {
                Guard.Against.Null(this.Id, nameof(this.Id));
            }
            catch (Exception exception)
            {
                throw new InvalidAggregateStateException<User, UserId>(this, exception.Message);
            }
        }

        private void Applier(UserCreatedEvent @event)
        {
            this.Id = new UserId(@event.AggregateId);
        }
    }
}