namespace JordiAragon.Cinema.Domain.AuditoriumAggregate
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class ShowtimeId : BaseValueObject
    {
        private ShowtimeId(Guid value)
        {
            this.Value = value;
        }

        public Guid Value { get; init; }

        public static ShowtimeId Create(Guid id)
        {
            Guard.Against.NullOrEmpty(id, nameof(id));

            return new ShowtimeId(id);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}