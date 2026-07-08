namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;

    public sealed class ShowtimeReadModelConfiguration : BaseReadModelTypeConfiguration<ShowtimeReadModel, Guid, ReservationReadModelContext>
    {
        public ShowtimeReadModelConfiguration(ReservationReadModelContext dbContext)
            : base(dbContext)
        {
        }
    }
}