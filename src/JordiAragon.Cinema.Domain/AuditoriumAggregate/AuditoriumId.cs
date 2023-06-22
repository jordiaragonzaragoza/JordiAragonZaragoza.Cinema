namespace JordiAragon.Cinema.Domain.AuditoriumAggregate
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Entities;

    public sealed class AuditoriumId : BaseAggregateRootId<Guid>
    {
        private AuditoriumId(Guid value)
        {
            this.Value = value;
        }

        public override Guid Value { get; protected set; }

        public static AuditoriumId Create(Guid id)
        {
            Guard.Against.NullOrEmpty(id, nameof(id));

            return new AuditoriumId(id);
        }
    }
}