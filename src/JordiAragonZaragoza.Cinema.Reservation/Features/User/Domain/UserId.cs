namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class UserId : BaseAggregateRootId<Guid>
    {
        private UserId(Guid value)
            : base(value)
        {
        }

        public static UserId Create(Guid id)
            => new(id);
    }
}