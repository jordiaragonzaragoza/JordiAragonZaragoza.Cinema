namespace JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Events;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain.Events;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Events;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain.Events;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain.Events;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain.Events;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore;

    public static class SeedData
    {
        public static readonly Movie ExampleMovie =
            Movie.Add(
                id: new MovieId(new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6")),
                title: Title.Create("Inception"),
                runtime: Runtime.Create(TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28)),
                exhibitionPeriod: ExhibitionPeriod.Create(
                    StartingPeriod.Create(new DateTimeOffset(DateTimeOffset.UtcNow.AddYears(1).Year, 1, 1, 1, 1, 1, TimeSpan.Zero)),
                    EndOfPeriod.Create(DateTimeOffset.UtcNow.AddYears(2)),
                    Runtime.Create(TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28))));

        public static readonly MovieAddedEvent ExampleMovieAddedEvent =
            new(
                ExampleMovie.Id,
                ExampleMovie.Title,
                ExampleMovie.Runtime,
                ExampleMovie.ExhibitionPeriod.StartingPeriodOnUtc,
                ExampleMovie.ExhibitionPeriod.EndOfPeriodOnUtc);

        public static readonly MovieReadModel ExampleMovieReadModel =
           new(
            ExampleMovie.Id,
            ExampleMovie.Title,
            ExampleMovie.Runtime);

        public static readonly Auditorium ExampleAuditorium =
            Auditorium.Create(
                id: new AuditoriumId(new Guid("c91aa0e0-9bc0-4db3-805c-23e3d8eabf53")),
                name: Name.Create("Auditorium One"),
                rows: Rows.Create(10),
                seatsPerRow: SeatsPerRow.Create(10));

        public static readonly AuditoriumCreatedEvent ExampleAuditoriumCreatedEvent =
            new(
                ExampleAuditorium.Id,
                ExampleAuditorium.Name,
                ExampleAuditorium.Rows,
                ExampleAuditorium.SeatsPerRow,
                ExampleAuditorium.Seats.Select(seat => (Guid)seat.Id).ToList().AsReadOnly(),
                ExampleAuditorium.Seats.Select(seat => (ushort)seat.Row).ToList().AsReadOnly(),
                ExampleAuditorium.Seats.Select(seat => (ushort)seat.SeatNumber).ToList().AsReadOnly());

        public static readonly AuditoriumReadModel ExampleAuditoriumReadModel =
            new(
                ExampleAuditorium.Id,
                ExampleAuditorium.Name,
                ExampleAuditorium.Seats.Select(seat => new SeatReadModel(
                    seat.Id,
                    seat.Row,
                    seat.SeatNumber)).ToList());

        public static readonly Cinema ExampleCinema =
            Cinema.Create(
                id: new CinemaId(new Guid("497cd8f1-620c-426f-992d-7012147f6e7c")));

        public static readonly CinemaCreatedEvent ExampleCinemaCreatedEvent =
            new(ExampleCinema.Id);

        public static readonly CinemaReadModel ExampleCinemaReadModel =
            new(ExampleCinema.Id);

        public static readonly Partition ExamplePartition =
            Partition.Create(
                id: new PartitionId(new Guid("05cbd871-da4e-447b-9d62-89438df8c4a8")));

        public static readonly PartitionCreatedEvent ExamplePartitionCreatedEvent =
            new(ExamplePartition.Id);

        public static readonly PartitionReadModel ExamplePartitionReadModel =
            new(ExamplePartition.Id);

        public static readonly Tenant ExampleTenant =
            Tenant.Create(
                id: new TenantId(new Guid("667196fc-0e17-43f9-990d-7dc2175e6162")));

        public static readonly TenantCreatedEvent ExampleTenantCreatedEvent =
            new(ExampleTenant.Id);

        public static readonly TenantReadModel ExampleTenantReadModel =
            new(ExampleTenant.Id);

        public static readonly User ExampleUser =
            User.Create(
                id: new UserId(new Guid("08ffddf5-3826-483f-a806-b3144477c7e8")));

        public static readonly UserCreatedEvent ExampleUserCreatedEvent =
            new(ExampleUser.Id);

        public static readonly UserReadModel ExampleUserReadModel =
            new(ExampleUser.Id);

        public static readonly Showtime ExampleShowtime =
            Showtime.Schedule(
                id: new ShowtimeId(new Guid("89b073a7-cfcf-4f2a-b01b-4c7f71a0563b")),
                movieId: ExampleMovie.Id,
                sessionDateOnUtc: SessionDate.Create(DateTimeOffset.UtcNow.AddYears(1)),
                auditoriumId: ExampleAuditorium.Id);

        public static readonly JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events.ShowtimeScheduledEvent ExampleShowtimeScheduledEvent =
            new(
                ExampleShowtime.Id,
                ExampleShowtime.MovieId,
                ExampleShowtime.SessionDateOnUtc,
                ExampleShowtime.AuditoriumId);

        public static readonly ShowtimeReadModel ExampleShowtimeReadModel =
            new(
                ExampleShowtime.Id,
                ExampleShowtime.SessionDateOnUtc,
                ExampleMovie.Id,
                ExampleMovie.Title,
                ExampleMovie.Runtime,
                ExampleAuditorium.Id,
                ExampleAuditorium.Name);

        public static readonly Reservation ExampleReservation =
            new(
                id: new ReservationId(new Guid("d290f1ee-6c54-4b01-90e6-d701748f0851")),
                userId: ExampleUser.Id,
                seatIds: ExampleAuditorium.Seats.Take(3).Select(seat => seat.Id),
                reservationDateOnUtc: ReservationDate.Create(DateTimeOffset.UtcNow.AddDays(1))); // Added 1 day to avoid conflicts with the seat reservation time in the showtime.

        public static readonly ReservedSeatsEvent ExampleReservedSeatsEvent =
            new(
                ExampleShowtime.Id,
                ExampleReservation.Id,
                ExampleReservation.UserId,
                ExampleReservation.Seats.Select(seatId => seatId.Value),
                ExampleReservation.ReservationDateOnUtc);

        public static ReadOnlyCollection<AvailableSeatReadModel> ExampleAvailableSeatsReadModel =>
            ExampleAuditorium.Seats
                .Where(seat => !ExampleReservation.Seats.Contains(seat.Id))
                .Select(seat => new AvailableSeatReadModel(
                    id: Guid.NewGuid(),
                    seatId: seat.Id,
                    row: seat.Row,
                    seatNumber: seat.SeatNumber,
                    showtimeId: ExampleShowtime.Id,
                    auditoriumId: ExampleAuditorium.Id,
                    auditoriumName: ExampleAuditorium.Name))
                .ToList()
                .AsReadOnly();

        public static ReservationReadModel ExampleReservationReadModel() =>
            new(
                id: ExampleReservation.Id,
                userId: ExampleUser.Id,
                showtimeId: ExampleShowtime.Id,
                sessionDateOnUtc: ExampleShowtime.SessionDateOnUtc,
                auditoriumName: ExampleAuditorium.Name,
                movieTitle: ExampleMovie.Title,
                seats: ExampleAuditorium.Seats.Take(3).Select(seat => new SeatReadModel(
                    seat.Id,
                    seat.Row,
                    seat.SeatNumber)).ToList(),
                isPurchased: false,
                createdTimeOnUtc: DateTimeOffset.UtcNow);

        public static UserAuthorizationReadModel ExampleUserAuthorizationReadModel()
        {
            var userAuthorization = new UserAuthorizationReadModel(ExampleUser.Id)
            {
                UserId = ExampleUser.Id,
                TenantId = ExampleTenant.Id,
                PartitionId = ExamplePartition.Id,
                CinemaId = ExampleCinema.Id,
                Roles = new List<RoleReadModel>
                {
                    new RoleReadModel(Guid.NewGuid(), "Admin"),
                },
            };

            return userAuthorization;
        }

        public static void PopulateReadModelTestData(ReservationReadModelContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            context.Movies.Add(ExampleMovieReadModel);

            context.Auditoriums.Add(ExampleAuditoriumReadModel);

            context.Users.Add(ExampleUserReadModel);

            context.UsersAuthorizations.Add(ExampleUserAuthorizationReadModel());

            context.Tenants.Add(ExampleTenantReadModel);

            context.Partitions.Add(ExamplePartitionReadModel);

            context.Cinemas.Add(ExampleCinemaReadModel);

            context.Showtimes.Add(ExampleShowtimeReadModel);

            context.Reservations.AddRange(ExampleReservationReadModel());

            context.AvailableSeats.AddRange(ExampleAvailableSeatsReadModel);

            context.SaveChanges();
        }

        public static async Task PopulateBusinessModelTestDataAsync(
            IEventStore eventStore,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(eventStore, nameof(eventStore));

            var hasExistingData = await HasExistingDataAsync(eventStore, cancellationToken);
            if (hasExistingData)
            {
                return;
            }

            eventStore.AppendChanges<Movie, MovieId>(ExampleMovie);
            eventStore.AppendChanges<Auditorium, AuditoriumId>(ExampleAuditorium);
            eventStore.AppendChanges<Partition, PartitionId>(ExamplePartition);
            eventStore.AppendChanges<Tenant, TenantId>(ExampleTenant);
            eventStore.AppendChanges<Cinema, CinemaId>(ExampleCinema);

            ExampleUser.GrantUser(
                new Scope(ExampleTenant.Id, ExamplePartition.Id, ExampleCinema.Id),
                new[] { Role.Create("Admin") });

            eventStore.AppendChanges<User, UserId>(ExampleUser);
            ExampleShowtime.ReserveSeats(
                ExampleReservation.Id,
                ExampleReservation.UserId,
                ExampleReservation.Seats,
                ExampleReservation.ReservationDateOnUtc);

            eventStore.AppendChanges<Showtime, ShowtimeId>(ExampleShowtime);

            await eventStore.SaveChangesAsync(cancellationToken);
        }

        private static async Task<bool> HasExistingDataAsync(IEventStore eventStore, CancellationToken cancellationToken)
        {
            var existingMovie = await eventStore.LoadAggregateAsync<Movie, MovieId>(
                                ExampleMovie.Id,
                                cancellationToken);

            var existingAuditorium = await eventStore.LoadAggregateAsync<Auditorium, AuditoriumId>(
                ExampleAuditorium.Id,
                cancellationToken);

            var existingUser = await eventStore.LoadAggregateAsync<User, UserId>(
                ExampleUser.Id,
                cancellationToken);

            var existingPartition = await eventStore.LoadAggregateAsync<Partition, PartitionId>(
                ExamplePartition.Id,
                cancellationToken);

            var existingTenant = await eventStore.LoadAggregateAsync<Tenant, TenantId>(
                ExampleTenant.Id,
                cancellationToken);

            var existingCinema = await eventStore.LoadAggregateAsync<Cinema, CinemaId>(
                ExampleCinema.Id,
                cancellationToken);

            var existingShowtime = await eventStore.LoadAggregateAsync<Showtime, ShowtimeId>(
                ExampleShowtime.Id,
                cancellationToken);

            if (existingMovie != null ||
                existingAuditorium != null ||
                existingUser != null ||
                existingPartition != null ||
                existingTenant != null ||
                existingCinema != null ||
                existingShowtime != null)
            {
                return true;
            }

            return false;
        }
    }
}