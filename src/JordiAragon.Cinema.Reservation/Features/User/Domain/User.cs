namespace JordiAragon.Cinema.Reservation.User.Domain
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.User.Domain.Events;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;
    using JordiAragon.SharedKernel.Domain.Exceptions;

    public sealed class User : BaseAggregateRoot<UserId, Guid>
    {
        // Required by EF.
        private User()
        {
        }

        public static User Create(
            UserId id)
        {
            var user = new User();

            user.Apply(new UserCreatedEvent(id));

            return user;
        }

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case UserCreatedEvent @event:
                    this.Applier(@event);
                    break;
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
            this.Id = UserId.Create(@event.AggregateId);
        }
    }
}