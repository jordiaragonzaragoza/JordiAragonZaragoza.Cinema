namespace JordiAragonZaragoza.Cinema.Reservation.User.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;

    public sealed class UserReadModelConfiguration : BaseModelTypeConfiguration<UserReadModel, Guid>
    {
    }
}