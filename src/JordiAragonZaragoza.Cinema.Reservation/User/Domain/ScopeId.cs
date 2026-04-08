namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class ScopeId : BaseValueObject, IEntityId<Scope>
    {
        public ScopeId(Scope value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            this.Value = value;
        }

        public Scope Value { get; init; }

        public static implicit operator Scope(ScopeId self)
        {
            ArgumentNullException.ThrowIfNull(self);

            return self.Value;
        }

        public Scope ToScope()
        {
            return this.Value;
        }

        public Scope FromScopeId()
        {
            return this.Value;
        }

        public override string? ToString()
        {
            return this.Value.ToString();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }
}