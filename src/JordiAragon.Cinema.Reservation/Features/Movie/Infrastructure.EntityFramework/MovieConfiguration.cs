namespace JordiAragon.Cinema.Reservation.Movie.Infrastructure.EntityFramework
{
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public sealed class MovieConfiguration : BaseAggregateRootTypeConfiguration<Movie, MovieId>
    {
        public override void Configure(EntityTypeBuilder<Movie> builder)
        {
            Guard.Against.Null(builder, nameof(builder));

            this.ConfigureMoviesTable(builder);

            ConfigureMoviesShowtimeIdsTable(builder);
        }

        private static void ConfigureMoviesShowtimeIdsTable(EntityTypeBuilder<Movie> builder)
        {
            builder.OwnsMany(movie => movie.ActiveShowtimes, sib =>
            {
                sib.ToTable("MoviesActiveShowtimeIds");

                sib.WithOwner().HasForeignKey(nameof(MovieId));

                sib.Property(showtimeId => showtimeId.Value)
                .ValueGeneratedNever()
                .HasColumnName(nameof(ShowtimeId));
            });

            builder.Metadata.FindNavigation(nameof(Movie.ActiveShowtimes))
                ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }

        private void ConfigureMoviesTable(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movies");

            base.Configure(builder);

            builder.Property(movie => movie.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => MovieId.Create(value));

            builder.Property(movie => movie.Runtime)
                .HasConversion(
                    runtime => runtime.Value,
                    value => Runtime.Create(value));

            builder.OwnsOne(movie => movie.ExhibitionPeriod, exhibitionBuilder =>
            {
                exhibitionBuilder.Property(x => x.StartingPeriodOnUtc)
                .HasColumnName("StartingExhibitionPeriodOnUtc")
                .HasConversion(
                    startingPeriod => startingPeriod.Value,
                    value => StartingPeriod.Create(value));

                exhibitionBuilder.Property(x => x.EndOfPeriodOnUtc)
                .HasColumnName("EndOfExhibitionPeriodOnUtc")
                .HasConversion(
                    endPeriod => endPeriod.Value,
                    value => EndOfPeriod.Create(value));
            }).Navigation(movie => movie.ExhibitionPeriod).IsRequired();
        }
    }
}