namespace JordiAragon.Cinema.Reservation.Showtime.Infrastructure.EntityFramework
{
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class AvailableSeatReadModelConfiguration : IEntityTypeConfiguration<AvailableSeatReadModel>
    {
        public void Configure(EntityTypeBuilder<AvailableSeatReadModel> builder)
        {
            builder.HasKey(avaliableSeatReadModel => avaliableSeatReadModel.Id);
        }
    }
}