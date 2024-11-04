namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class UserId : BaseAggregateRootId<Guid>
    {
        public UserId(Guid value)
            : base(value)
        {
        }
    }
}