namespace JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class TenantId : BaseAggregateRootId<Guid>
    {
        public TenantId(Guid value)
            : base(value)
        {
        }
    }
}