namespace JordiAragon.Cinema.Reservation.User.Infrastructure.EntityFramework
{
    using JordiAragon.Cinema.Reservation.User.Domain;
    using JordiAragon.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public sealed class UserConfiguration : BaseEntityTypeConfiguration<User, UserId>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            this.ConfigureUsersTable(builder);
        }

        private void ConfigureUsersTable(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            base.Configure(builder);

            builder.Property(user => user.Id)
                .ValueGeneratedNever()
                .HasConversion(id => id.Value, value => UserId.Create(value));
        }
    }
}