namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Infrastructure.EntityFramework
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;

    public sealed class AuditoriumReadModelConfiguration : BaseModelTypeConfiguration<AuditoriumReadModel, Guid>
    {
    }
}