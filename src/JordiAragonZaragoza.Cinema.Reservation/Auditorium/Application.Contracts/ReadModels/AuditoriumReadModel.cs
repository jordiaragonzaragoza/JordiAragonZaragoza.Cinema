namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class AuditoriumReadModel : IReadModel
    {
        public AuditoriumReadModel(
            Guid id,
            string name)
        {
            this.Id = Guard.Against.Default(id, nameof(id));
            this.Name = Guard.Against.Default(name, nameof(name));
        }

        // Required by EF.
        private AuditoriumReadModel()
        {
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; } = default!;
    }
}