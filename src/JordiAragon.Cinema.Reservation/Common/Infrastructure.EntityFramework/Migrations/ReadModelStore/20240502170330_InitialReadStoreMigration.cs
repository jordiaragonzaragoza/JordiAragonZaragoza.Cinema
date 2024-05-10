using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Migrations.ReadModelStore
{
    /// <inheritdoc />
    public partial class InitialReadStoreMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvailableSeats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Row = table.Column<short>(type: "smallint", nullable: false),
                    SeatNumber = table.Column<short>(type: "smallint", nullable: false),
                    ShowtimeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditoriumId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditoriumName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableSeats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Showtimes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionDateOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    MovieId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovieTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovieRuntime = table.Column<TimeSpan>(type: "time", nullable: false),
                    AuditoriumId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditoriumName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Showtimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShowtimeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionDateOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AuditoriumName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MovieTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPurchased = table.Column<bool>(type: "bit", nullable: false),
                    CreatedTimeOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketsSeats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Row = table.Column<short>(type: "smallint", nullable: false),
                    SeatNumber = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketsSeats", x => new { x.Id, x.TicketId });
                    table.ForeignKey(
                        name: "FK_TicketsSeats_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketsSeats_TicketId",
                table: "TicketsSeats",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailableSeats");

            migrationBuilder.DropTable(
                name: "Showtimes");

            migrationBuilder.DropTable(
                name: "TicketsSeats");

            migrationBuilder.DropTable(
                name: "Tickets");
        }
    }
}
