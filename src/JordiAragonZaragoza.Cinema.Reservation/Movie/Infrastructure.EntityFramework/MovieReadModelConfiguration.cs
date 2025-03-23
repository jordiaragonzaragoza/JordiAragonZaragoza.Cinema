namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Infrastructure.EntityFramework
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;

    public sealed class MovieReadModelConfiguration : BaseModelTypeConfiguration<MovieReadModel, Guid>
    {
    }
}