namespace JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class TicketId : BaseEntityId<Guid>
    {
        private TicketId(Guid value)
            : base(value)
        {
        }

        public static TicketId Create(Guid id)
        {
            Guard.Against.NullOrEmpty(id, nameof(id));

            return new TicketId(id);
        }
    }
}