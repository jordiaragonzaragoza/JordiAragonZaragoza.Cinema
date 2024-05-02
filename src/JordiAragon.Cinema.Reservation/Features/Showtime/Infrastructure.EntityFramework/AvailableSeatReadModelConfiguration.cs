namespace JordiAragon.Cinema.Reservation.Showtime.Infrastructure.EntityFramework
{
    using System;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;

    public sealed class AvailableSeatReadModelConfiguration : BaseModelTypeConfiguration<AvailableSeatReadModel, Guid>
    {
    }
}