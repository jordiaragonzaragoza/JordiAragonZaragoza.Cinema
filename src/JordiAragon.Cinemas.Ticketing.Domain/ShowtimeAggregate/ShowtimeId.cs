namespace JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class ShowtimeId : BaseAggregateRootId<Guid>
    {
        private ShowtimeId(Guid value)
            : base(value)
        {
        }

        public static ShowtimeId Create(Guid id)
        {
            Guard.Against.NullOrEmpty(id, nameof(id));

            return new ShowtimeId(id);
        }
    }
}