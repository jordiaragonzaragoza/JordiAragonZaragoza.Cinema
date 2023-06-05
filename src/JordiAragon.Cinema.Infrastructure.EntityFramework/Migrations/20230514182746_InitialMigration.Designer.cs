﻿// <auto-generated />
using System;
using JordiAragon.Cinema.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JordiAragon.Cinema.Infrastructure.EntityFramework.Migrations
{
    [DbContext(typeof(CinemaContext))]
    [Migration("20230514182746_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JordiAragon.Cinema.Domain.AuditoriumAggregate.Auditorium", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDateOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModificationDateOnUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Auditoriums", (string)null);
                });

            modelBuilder.Entity("JordiAragon.Cinema.Domain.AuditoriumAggregate.Seat", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuditoriumId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDateOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModificationDateOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<short>("Row")
                        .HasColumnType("smallint");

                    b.Property<short>("SeatNumber")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("AuditoriumId");

                    b.ToTable("Seats", (string)null);
                });

            modelBuilder.Entity("JordiAragon.Cinema.Domain.AuditoriumAggregate.Showtime", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AuditoriumId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDateOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModificationDateOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("SessionDateOnUtc")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuditoriumId");

                    b.HasIndex("MovieId");

                    b.ToTable("Showtimes", (string)null);
                });

            modelBuilder.Entity("JordiAragon.Cinema.Domain.AuditoriumAggregate.Ticket", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTimeOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreationDateOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModificationDateOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ShowtimeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ShowtimeId");

                    b.ToTable("Tickets", (string)null);
                });

            modelBuilder.Entity("JordiAragon.Cinema.Domain.AuditoriumAggregate.TicketSeat", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDateOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModificationDateOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("SeatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TicketId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SeatId");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketSeats", (string)null);
                });

            modelBuilder.Entity("JordiAragon.Cinema.Domain.MovieAggregate.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDateOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImdbId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastModifiedByUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModificationDateOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ReleaseDateOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Stars")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Movies", (string)null);
                });

            modelBuilder.Entity("JordiAragon.SharedKernel.Infrastructure.EntityFramework.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOccurredOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateProcessedOnUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Error")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessages");
                });

            modelBuilder.Entity("JordiAragon.Cinema.Domain.AuditoriumAggregate.Seat", b =>
                {
                    b.HasOne("JordiAragon.Cinema.Domain.AuditoriumAggregate.Auditorium", null)
                        .WithMany("Seats")
                        .HasForeignKey("AuditoriumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JordiAragon.Cinema.Domain.AuditoriumAggregate.Showtime", b =>
                {
                    b.HasOne("JordiAragon.Cinema.Domain.AuditoriumAggregate.Auditorium", null)
                        .WithMany("Showtimes")
                        .HasForeignKey("AuditoriumId");

                    b.HasOne("JordiAragon.Cinema.Domain.MovieAggregate.Movie", "Movie")
                        .WithMany("Showtimes")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("JordiAragon.Cinema.Domain.AuditoriumAggregate.Ticket", b =>
                {
                    b.HasOne("JordiAragon.Cinema.Domain.AuditoriumAggregate.Showtime", null)
                        .WithMany("Tickets")
                        .HasForeignKey("ShowtimeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JordiAragon.Cinema.Domain.AuditoriumAggregate.TicketSeat", b =>
                {
                    b.HasOne("JordiAragon.Cinema.Domain.AuditoriumAggregate.Seat", null)
                        .WithMany("Tickets")
                        .HasForeignKey("SeatId");

                    b.HasOne("JordiAragon.Cinema.Domain.AuditoriumAggregate.Ticket", null)
                        .WithMany("Seats")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JordiAragon.Cinema.Domain.AuditoriumAggregate.Auditorium", b =>
                {
                    b.Navigation("Seats");

                    b.Navigation("Showtimes");
                });

            modelBuilder.Entity("JordiAragon.Cinema.Domain.AuditoriumAggregate.Seat", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("JordiAragon.Cinema.Domain.AuditoriumAggregate.Showtime", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("JordiAragon.Cinema.Domain.AuditoriumAggregate.Ticket", b =>
                {
                    b.Navigation("Seats");
                });

            modelBuilder.Entity("JordiAragon.Cinema.Domain.MovieAggregate.Movie", b =>
                {
                    b.Navigation("Showtimes");
                });
#pragma warning restore 612, 618
        }
    }
}
