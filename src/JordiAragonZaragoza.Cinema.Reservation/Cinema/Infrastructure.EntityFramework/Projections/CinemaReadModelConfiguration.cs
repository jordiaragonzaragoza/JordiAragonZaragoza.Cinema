namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;

    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;

    public sealed class CinemaReadModelConfiguration : BaseReadModelTypeConfiguration<CinemaReadModel, Guid, ReservationReadModelContext>
    {
        public CinemaReadModelConfiguration(ReservationReadModelContext dbContext)
            : base(dbContext)
        {
        }
    }
}