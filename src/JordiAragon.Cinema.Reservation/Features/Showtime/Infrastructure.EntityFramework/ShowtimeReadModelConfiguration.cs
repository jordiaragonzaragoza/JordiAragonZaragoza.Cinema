namespace JordiAragon.Cinema.Reservation.Showtime.Infrastructure.EntityFramework
{
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ShowtimeReadModelConfiguration : IEntityTypeConfiguration<ShowtimeReadModel>
    {
        public void Configure(EntityTypeBuilder<ShowtimeReadModel> builder)
        {
            builder.HasKey(showtimeReadModel => showtimeReadModel.Id);
        }
    }
}