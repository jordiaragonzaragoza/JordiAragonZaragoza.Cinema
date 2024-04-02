﻿// <auto-generated />
using System;
using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Migrations.ReadModelStore
{
    [DbContext(typeof(ReservationReadModelContext))]
    [Migration("20240328175525_InitialReadStoreMigration")]
    partial class InitialReadStoreMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels.AvailableSeatReadModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuditoriumId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AuditoriumName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<short>("Row")
                        .HasColumnType("smallint");

                    b.Property<Guid>("SeatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<short>("SeatNumber")
                        .HasColumnType("smallint");

                    b.Property<Guid>("ShowtimeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("AvailableSeats");
                });

            modelBuilder.Entity("JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels.ShowtimeReadModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuditoriumId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AuditoriumName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeSpan>("MovieRuntime")
                        .HasColumnType("time");

                    b.Property<string>("MovieTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("SessionDateOnUtc")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("Showtimes");
                });

            modelBuilder.Entity("JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels.TicketReadModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AuditoriumName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPurchased")
                        .HasColumnType("bit");

                    b.Property<string>("MovieTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("SessionDateOnUtc")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("ShowtimeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Tickets", (string)null);
                });

            modelBuilder.Entity("JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels.TicketReadModel", b =>
                {
                    b.OwnsMany("JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels.SeatReadModel", "Seats", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("TicketId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<short>("Row")
                                .HasColumnType("smallint");

                            b1.Property<short>("SeatNumber")
                                .HasColumnType("smallint");

                            b1.HasKey("Id", "TicketId");

                            b1.HasIndex("TicketId");

                            b1.ToTable("TicketsSeats", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("TicketId");
                        });

                    b.Navigation("Seats");
                });
#pragma warning restore 612, 618
        }
    }
}
