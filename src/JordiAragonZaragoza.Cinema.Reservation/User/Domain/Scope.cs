namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class Scope : BaseValueObject
    {
        internal Scope(TenantId tenantId, PartitionId? partitionId, CinemaId? cinemaId)
        {
            this.TenantId = tenantId;
            this.PartitionId = partitionId;
            this.CinemaId = cinemaId;
        }

        // Required by EF.
        private Scope()
        {
        }

        public TenantId TenantId { get; init; } = default!;

        public PartitionId? PartitionId { get; init; }

        public CinemaId? CinemaId { get; init; }

        public static Scope Create(TenantId tenantId, PartitionId? partitionId = default, CinemaId? cinemaId = default)
        {
            ArgumentNullException.ThrowIfNull(tenantId, nameof(tenantId));

            return new Scope(tenantId, partitionId, cinemaId);
        }

        public bool Matches(CinemaId? cinemaId, PartitionId? partitionId, TenantId tenantId)
        {
            if (this.CinemaId is not null)
            {
                return this.CinemaId == cinemaId!;
            }

            if (this.PartitionId is not null)
            {
                return this.PartitionId == partitionId!;
            }

            return this.TenantId == tenantId;
        }

        public override string ToString()
            => $"TenantId: {this.TenantId} PartitionId: {this.PartitionId} CinemaId: {this.CinemaId}";

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.TenantId;
            yield return this.PartitionId ?? (object)"null";
            yield return this.CinemaId ?? (object)"null";
        }
    }
}