namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;

    public sealed class ShowtimeReadModelConfiguration : BaseModelTypeConfiguration<ShowtimeReadModel, Guid>
    {
    }
}