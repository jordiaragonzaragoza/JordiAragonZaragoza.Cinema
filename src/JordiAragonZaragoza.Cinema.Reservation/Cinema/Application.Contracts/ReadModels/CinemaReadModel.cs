namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.ReadModels
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class CinemaReadModel : IReadModel
    {
        public CinemaReadModel(
            Guid id)
        {
            this.Id = Guard.Against.Default(id, nameof(id));
        }

        // Required by EF.
        private CinemaReadModel()
        {
        }

        public Guid Id { get; private set; }
    }
}