namespace JordiAragon.Cinema.Infrastructure.EntityFramework.Configurations
{
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ShowtimeConfiguration : BaseEntityTypeConfiguration<Showtime, ShowtimeId>
    {
        public override void Configure(EntityTypeBuilder<Showtime> builder)
        {
            base.Configure(builder);

            builder.ToTable("Showtimes");

            builder.Property(showtime => showtime.Id)
                .ValueGeneratedNever()
                .HasConversion(showtimeId => showtimeId.Value, intValue => ShowtimeId.Create(intValue));

            builder.HasOne<Movie>()
                    .WithMany(entry => entry.Showtimes)
                    .HasForeignKey(showtime => showtime.MovieId)
                    .IsRequired();

            builder.HasOne(entry => entry.Auditorium)
                    .WithMany(entry => entry.Showtimes)
                    .IsRequired();

            builder.HasMany(showtime => showtime.Tickets)
                .WithOne()
                .HasForeignKey(ticket => ticket.ShowtimeId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}