namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;

    public sealed class MovieReadModelConfiguration : BaseReadModelTypeConfiguration<MovieReadModel, Guid, ReservationReadModelContext>
    {
        public MovieReadModelConfiguration(ReservationReadModelContext dbContext)
            : base(dbContext)
        {
        }
    }
}