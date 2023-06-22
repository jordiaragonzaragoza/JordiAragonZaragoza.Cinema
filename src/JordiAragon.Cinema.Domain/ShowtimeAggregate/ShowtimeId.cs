namespace JordiAragon.Cinema.Domain.ShowtimeAggregate
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.Entities;

    public sealed class ShowtimeId : BaseAggregateRootId<Guid>
    {
        private ShowtimeId(Guid value)
        {
            this.Value = value;
        }

        public override Guid Value { get; protected set; }

        public static ShowtimeId Create(Guid id)
        {
            Guard.Against.NullOrEmpty(id, nameof(id));

            return new ShowtimeId(id);
        }
    }
}