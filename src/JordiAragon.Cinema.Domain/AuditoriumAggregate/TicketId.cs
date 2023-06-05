namespace JordiAragon.Cinema.Domain.AuditoriumAggregate
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class TicketId : BaseValueObject
    {
        private TicketId(Guid value)
        {
            this.Value = value;
        }

        public Guid Value { get; init; }

        public static TicketId Create(Guid id)
        {
            Guard.Against.NullOrEmpty(id, nameof(id));

            return new TicketId(id);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}