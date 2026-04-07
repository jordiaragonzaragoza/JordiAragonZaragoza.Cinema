namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;

    public sealed class CinemaReadModelConfiguration : BaseModelTypeConfiguration<CinemaReadModel, Guid>
    {
    }
}