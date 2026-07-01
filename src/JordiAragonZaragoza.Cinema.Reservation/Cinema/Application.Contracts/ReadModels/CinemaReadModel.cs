namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.ReadModels
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Application.ReadModels;

    public sealed class CinemaReadModel : BaseReadModel
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
    }
}