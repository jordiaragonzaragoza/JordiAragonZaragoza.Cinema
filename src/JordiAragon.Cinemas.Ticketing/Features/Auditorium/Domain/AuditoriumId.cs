namespace JordiAragon.Cinemas.Ticketing.Auditorium.Domain
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Domain.ValueObjects;

    public sealed class AuditoriumId : BaseAggregateRootId<Guid>
    {
        private AuditoriumId(Guid value)
            : base(value)
        {
        }

        public static AuditoriumId Create(Guid id)
        {
            Guard.Against.NullOrEmpty(id, nameof(id));

            return new AuditoriumId(id);
        }
    }
}