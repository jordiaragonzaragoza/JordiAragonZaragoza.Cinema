namespace JordiAragon.Cinema.Domain.ShowtimeAggregate
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class TicketSeatId : BaseValueObject
    {
        private TicketSeatId(Guid value)
        {
            this.Value = Guard.Against.NullOrEmpty(value, nameof(value));
        }

        public Guid Value { get; init; }

        public static TicketSeatId Create(Guid id)
        {
            Guard.Against.NullOrEmpty(id, nameof(id));

            return new TicketSeatId(id);
        }

        // TODO: Review. Better pass a guid from generator.
        public static TicketSeatId CreateUnique()
        {
            return new TicketSeatId(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}