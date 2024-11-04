namespace JordiAragonZaragoza.Cinema.Reservation.User.Infrastructure.EntityFramework
{
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public sealed class UserConfiguration : BaseAggregateRootTypeConfiguration<User, UserId>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            Guard.Against.Null(builder, nameof(builder));

            this.ConfigureUsersTable(builder);
        }

        private void ConfigureUsersTable(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            base.Configure(builder);

            builder.Property(user => user.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => new UserId(value));
        }
    }
}