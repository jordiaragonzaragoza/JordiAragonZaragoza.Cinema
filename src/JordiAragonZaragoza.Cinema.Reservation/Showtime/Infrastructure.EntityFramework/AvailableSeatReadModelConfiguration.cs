namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Infrastructure.EntityFramework
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;

    public sealed class AvailableSeatReadModelConfiguration : BaseModelTypeConfiguration<AvailableSeatReadModel, Guid>
    {
    }
}