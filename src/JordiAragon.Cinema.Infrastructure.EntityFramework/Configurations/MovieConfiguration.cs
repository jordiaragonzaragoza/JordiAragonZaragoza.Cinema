namespace JordiAragon.Cinema.Infrastructure.EntityFramework.Configurations
{
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MovieConfiguration : BaseEntityTypeConfiguration<Movie, MovieId>
    {
        public override void Configure(EntityTypeBuilder<Movie> builder)
        {
            base.Configure(builder);

            builder.ToTable("Movies");

            builder.Property(entry => entry.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => MovieId.Create(value));
        }
    }
}