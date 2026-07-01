using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections.Migrations
{
    /// <inheritdoc />
    public partial class InitialReadModelMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "__Checkpoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    CheckpointedAtOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK___Checkpoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auditoriums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Scope_TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Scope_PartitionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Scope_DomainId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditoriums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AvailableSeats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SeatId = table.Column<Guid>(type: "uuid", nullable: false),
                    Row = table.Column<int>(type: "integer", nullable: false),
                    SeatNumber = table.Column<int>(type: "integer", nullable: false),
                    ShowtimeId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuditoriumId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuditoriumName = table.Column<string>(type: "text", nullable: false),
                    Scope_TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Scope_PartitionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Scope_DomainId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableSeats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cinemas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Scope_TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Scope_PartitionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Scope_DomainId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cinemas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Runtime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Scope_TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Scope_PartitionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Scope_DomainId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Partitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Scope_TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Scope_PartitionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Scope_DomainId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShowtimeId = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionDateOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AuditoriumName = table.Column<string>(type: "text", nullable: false),
                    MovieTitle = table.Column<string>(type: "text", nullable: false),
                    IsPurchased = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedTimeOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Scope_TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Scope_PartitionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Scope_DomainId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Showtimes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SessionDateOnUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    MovieId = table.Column<Guid>(type: "uuid", nullable: false),
                    MovieTitle = table.Column<string>(type: "text", nullable: false),
                    MovieRuntime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    AuditoriumId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuditoriumName = table.Column<string>(type: "text", nullable: false),
                    Scope_TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Scope_PartitionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Scope_DomainId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Showtimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Scope_TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Scope_PartitionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Scope_DomainId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Scope_TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Scope_PartitionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Scope_DomainId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsersAuthorizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    PartitionId = table.Column<Guid>(type: "uuid", nullable: true),
                    DomainId = table.Column<Guid>(type: "uuid", nullable: true),
                    Scope_TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Scope_PartitionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Scope_DomainId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersAuthorizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditoriumSeats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuditoriumId = table.Column<Guid>(type: "uuid", nullable: false),
                    Row = table.Column<int>(type: "integer", nullable: false),
                    SeatNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditoriumSeats", x => new { x.Id, x.AuditoriumId });
                    table.ForeignKey(
                        name: "FK_AuditoriumSeats_Auditoriums_AuditoriumId",
                        column: x => x.AuditoriumId,
                        principalTable: "Auditoriums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationsSeats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReservationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Row = table.Column<int>(type: "integer", nullable: false),
                    SeatNumber = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationsSeats", x => new { x.Id, x.ReservationId });
                    table.ForeignKey(
                        name: "FK_ReservationsSeats_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAuthorizationPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserAuthorizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionValue = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthorizationPermissions", x => new { x.Id, x.UserAuthorizationId });
                    table.ForeignKey(
                        name: "FK_UserAuthorizationPermissions_UsersAuthorizations_UserAuthor~",
                        column: x => x.UserAuthorizationId,
                        principalTable: "UsersAuthorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAuthorizationRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserAuthorizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleValue = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAuthorizationRoles", x => new { x.Id, x.UserAuthorizationId });
                    table.ForeignKey(
                        name: "FK_UserAuthorizationRoles_UsersAuthorizations_UserAuthorizatio~",
                        column: x => x.UserAuthorizationId,
                        principalTable: "UsersAuthorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditoriumSeats_AuditoriumId",
                table: "AuditoriumSeats",
                column: "AuditoriumId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationsSeats_ReservationId",
                table: "ReservationsSeats",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorizationPermissions_UserAuthorizationId",
                table: "UserAuthorizationPermissions",
                column: "UserAuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuthorizationRoles_UserAuthorizationId",
                table: "UserAuthorizationRoles",
                column: "UserAuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersAuthorizations_UserId",
                table: "UsersAuthorizations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersAuthorizations_UserIdScope",
                table: "UsersAuthorizations",
                columns: new[] { "UserId", "TenantId", "PartitionId", "DomainId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__Checkpoints");

            migrationBuilder.DropTable(
                name: "AuditoriumSeats");

            migrationBuilder.DropTable(
                name: "AvailableSeats");

            migrationBuilder.DropTable(
                name: "Cinemas");

            migrationBuilder.DropTable(
                name: "Movies");

            migrationBuilder.DropTable(
                name: "Partitions");

            migrationBuilder.DropTable(
                name: "ReservationsSeats");

            migrationBuilder.DropTable(
                name: "Showtimes");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "UserAuthorizationPermissions");

            migrationBuilder.DropTable(
                name: "UserAuthorizationRoles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Auditoriums");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "UsersAuthorizations");
        }
    }
}
