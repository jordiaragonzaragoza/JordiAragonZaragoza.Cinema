namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;

    public sealed class AvailableSeatReadModelConfiguration : BaseReadModelTypeConfiguration<AvailableSeatReadModel, Guid, ReservationReadModelContext>
    {
        public AvailableSeatReadModelConfiguration(ReservationReadModelContext dbContext)
            : base(dbContext)
        {
        }
    }
}