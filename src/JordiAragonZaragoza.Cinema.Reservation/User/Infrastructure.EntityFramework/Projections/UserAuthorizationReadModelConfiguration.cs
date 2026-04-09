namespace JordiAragonZaragoza.Cinema.Reservation.User.Infrastructure.EntityFramework.Projections
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// Configures the UserAuthorizationReadModel for Entity Framework Core.
    /// Uses the owned entities pattern to support future database portability.
    /// Each record represents a single user-scope authorization assignment.
    /// A user can have multiple records if granted roles at different scope levels (tenant, partition, cinema).
    /// </summary>
    public sealed class UserAuthorizationReadModelConfiguration : BaseModelTypeConfiguration<UserAuthorizationReadModel, Guid>
    {
        public override void Configure(EntityTypeBuilder<UserAuthorizationReadModel> builder)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));

            base.Configure(builder);

            ConfigureUserAuthorizationTable(builder);
        }

        private static void ConfigureUserAuthorizationTable(EntityTypeBuilder<UserAuthorizationReadModel> builder)
        {
            builder.ToTable("UsersAuthorizations");

            builder.HasKey(x => x.Id);

            // Configure UserId as an additional index for faster lookups by user
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_UsersAuthorizations_UserId");

            // Configure composite index for scope-based lookups
            builder.HasIndex(x => new { x.UserId, x.TenantId, x.PartitionId, x.CinemaId })
                .HasDatabaseName("IX_UsersAuthorizations_UserIdScope")
                .IsUnique();

            // Configure owned roles collection using owned entities pattern
            // This approach is database-agnostic and provides flexibility for future migrations
            builder.OwnsMany(userAuthorization => userAuthorization.Roles, sb =>
            {
                sb.ToTable("UserAuthorizationRoles");

                sb.WithOwner().HasForeignKey(nameof(UserAuthorizationReadModel.UserId));

                sb.HasKey(nameof(RoleReadModel.Id), nameof(UserAuthorizationReadModel.UserId));

                sb.Property(x => x.Value)
                    .HasColumnName("RoleValue")
                    .IsRequired();
            });

            builder.Metadata.FindNavigation(nameof(UserAuthorizationReadModel.Roles))
                ?.SetPropertyAccessMode(PropertyAccessMode.Field);

            // Document the scope hierarchy for clarity
            builder.Property(x => x.TenantId).IsRequired();
            builder.Property(x => x.PartitionId).IsRequired(false);
            builder.Property(x => x.CinemaId).IsRequired(false);
        }
    }
}