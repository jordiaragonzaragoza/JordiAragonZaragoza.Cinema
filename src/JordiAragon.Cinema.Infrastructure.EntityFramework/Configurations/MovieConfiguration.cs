namespace JordiAragon.Cinema.Infrastructure.EntityFramework.Configurations
{
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

            ConfigureMovieShowtimeIdsTable(builder);
        }

        private static void ConfigureMovieShowtimeIdsTable(EntityTypeBuilder<Movie> builder)
        {
            builder.OwnsMany(movie => movie.Showtimes, sib =>
            {
                sib.ToTable("MovieShowtimeIds");

                sib.WithOwner().HasForeignKey(nameof(MovieId));

                sib.HasKey("Id");

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