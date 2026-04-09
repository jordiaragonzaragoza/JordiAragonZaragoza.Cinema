namespace JordiAragonZaragoza.Cinema.Reservation.User.Infrastructure.EntityFramework.Projections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EntityFramework.Configuration;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <summary>
    /// Configures the UserAuthorizationReadModel for Entity Framework Core with PostgreSQL.
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
            // Table structure:
            // Id: Primary key (composite of UserId + Scope)
            // UserId: Foreign key reference to the user
            // TenantId, PartitionId, CinemaId: Define the scope hierarchy
            // Roles: JSON array stored in PostgreSQL jsonb column for optimal query performance
            builder.ToTable("UsersAuthorizations");

            // Configure primary key
            builder.HasKey(x => x.Id);

            // Configure UserId as an additional index for faster lookups by user
            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("IX_UsersAuthorizations_UserId");

            // Configure composite index for scope-based lookups
            builder.HasIndex(x => new { x.UserId, x.TenantId, x.PartitionId, x.CinemaId })
                .HasDatabaseName("IX_UsersAuthorizations_UserIdScope")
                .IsUnique();

            // Configure the Roles as a JSON column in PostgreSQL for optimal performance
            // This avoids the need for a separate normalized table and improves query performance
            builder.Property(x => x.Roles)
                .HasColumnType("jsonb")
                .HasDefaultValueSql("'[]'::jsonb")
                .Metadata.SetValueComparer(
                    new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<IList<string>>(
                        (a, b) => (a ?? new List<string>()).SequenceEqual(b ?? new List<string>()),
#pragma warning disable CA1307 // Use overload with StringComparison - EF Core value comparer requires specific hash behavior
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
#pragma warning restore CA1307
                        c => new List<string>(c ?? new List<string>())));

            // Document the scope hierarchy for clarity
            builder.Property(x => x.TenantId).IsRequired();
            builder.Property(x => x.PartitionId).IsRequired(false);
            builder.Property(x => x.CinemaId).IsRequired(false);
        }
    }
}