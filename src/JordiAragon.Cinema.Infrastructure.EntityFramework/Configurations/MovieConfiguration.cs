namespace JordiAragon.Cinema.Infrastructure.EntityFramework.Configurations
{
    using System;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MovieConfiguration : BaseEntityTypeConfiguration<Movie, MovieId>
    {
        public override void Configure(EntityTypeBuilder<Movie> builder)
        {
            this.ConfigureMoviesTable(builder);

            this.ConfigureMovieShowtimeIdsTable(builder);
        }

        private void ConfigureMoviesTable(EntityTypeBuilder<Movie> builder)
        {
            base.Configure(builder);

            builder.ToTable("Movies");

            builder.Property(movie => movie.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => MovieId.Create(value));
        }

        private void ConfigureMovieShowtimeIdsTable(EntityTypeBuilder<Movie> builder)
        {
            builder.OwnsMany(movie => movie.Showtimes, sib =>
            {
                sib.ToTable("MovieShowtimeIds");

                sib.WithOwner().HasForeignKey(nameof(MovieId));

                sib.HasKey(nameof(Showtime.Id));

                sib.Property(showtimeId => showtimeId.Value)
                .ValueGeneratedNever()
                .HasColumnName(nameof(ShowtimeId));
            });

            builder.Metadata.FindNavigation(nameof(Movie.Showtimes))
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}