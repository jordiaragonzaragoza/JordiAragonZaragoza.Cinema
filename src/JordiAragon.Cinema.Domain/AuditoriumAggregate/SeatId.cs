namespace JordiAragon.Cinema.Domain.AuditoriumAggregate
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class SeatId : BaseValueObject
    {
        private SeatId(Guid value)
        {
            this.Value = value;
        }

        public Guid Value { get; init; }

        public static SeatId Create(Guid id)
        {
            Guard.Against.NullOrEmpty(id, nameof(id));

            return new SeatId(id);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}