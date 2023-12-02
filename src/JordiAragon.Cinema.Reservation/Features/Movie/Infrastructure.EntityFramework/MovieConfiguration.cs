namespace JordiAragon.Cinema.Reservation.Movie.Infrastructure.EntityFramework
{
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MovieConfiguration : BaseEntityTypeConfiguration<Movie, MovieId>
    {
        public override void Configure(EntityTypeBuilder<Movie> builder)
        {
            this.ConfigureMoviesTable(builder);

            ConfigureMoviesShowtimeIdsTable(builder);
        }

        private static void ConfigureMoviesShowtimeIdsTable(EntityTypeBuilder<Movie> builder)
        {
            builder.OwnsMany(movie => movie.Showtimes, sib =>
            {
                sib.ToTable("MoviesShowtimeIds");

                sib.WithOwner().HasForeignKey(nameof(MovieId));

                sib.Property(showtimeId => showtimeId.Value)
                .ValueGeneratedNever()
                .HasColumnName(nameof(ShowtimeId));
            });

            builder.Metadata.FindNavigation(nameof(Movie.Showtimes))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureMoviesTable(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movies");

            base.Configure(builder);

            builder.Property(movie => movie.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => MovieId.Create(value));
        }
    }
}