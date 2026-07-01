namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Application.ReadModels;

    public sealed class UserReadModel : BaseReadModel
    {
        public UserReadModel(
            Guid id)
        {
            this.Id = Guard.Against.Default(id, nameof(id));
        }

        // Required by EF.
        private UserReadModel()
        {
        }
    }
}