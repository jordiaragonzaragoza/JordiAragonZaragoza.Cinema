namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Application.ReadModels;

    public sealed class RoleReadModel : BaseOwnedReadModel
    {
        public RoleReadModel(
            Guid id,
            string value)
        {
            this.Id = Guard.Against.Default(id, nameof(id));
            this.Value = Guard.Against.NullOrWhiteSpace(value, nameof(value));
        }

        // Required by EF.
        private RoleReadModel()
        {
        }

        public string Value { get; private set; } = default!;
    }
}