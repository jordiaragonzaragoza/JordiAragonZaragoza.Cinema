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
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Application;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Infrastructure.EventStore;

    /// <summary>
    /// Deterministic seed data for the system and functional tests.
    /// Every instance builds its own aggregates and read models, so no mutable state is shared
    /// between populations. Identifiers are stable and exposed through <see cref="SeedValues"/>.
    /// Use ONE instance per population: the read models receive their Scope when populated
    /// and EF Core tracks them.
    /// </summary>
    public sealed class SeedData
    {
        private readonly DateTimeOffset now;

        public SeedData()
            : this(TimeProvider.System)
        {
        }

        public SeedData(TimeProvider timeProvider)
        {
            ArgumentNullException.ThrowIfNull(timeProvider, nameof(timeProvider));

            this.now = timeProvider.GetUtcNow();

            var runtime = Runtime.Create(TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28));

            this.SystemTenant = Tenant.Create(
                id: new TenantId(SystemConstants.SystemTenantId));
            this.SystemTenantReadModel = new(this.SystemTenant.Id);

            this.ExampleTenant = Tenant.Create(
                id: new TenantId(SeedValues.ExampleTenantId));
            this.ExampleTenantReadModel = new(this.ExampleTenant.Id);

            this.ExamplePartition = Partition.Create(
                id: new PartitionId(SeedValues.ExamplePartitionId));
            this.ExamplePartitionReadModel = new(this.ExamplePartition.Id);

            this.ExampleCinema = Cinema.Create(
                id: new CinemaId(SeedValues.ExampleCinemaId));
            this.ExampleCinemaReadModel = new(this.ExampleCinema.Id);

            this.ExampleMovie = Movie.Add(
                id: new MovieId(SeedValues.ExampleMovieId),
                title: Title.Create(SeedValues.ExampleMovieTitle),
                runtime: runtime,
                exhibitionPeriod: ExhibitionPeriod.Create(
                    StartingPeriod.Create(new DateTimeOffset(this.now.AddYears(1).Year, 1, 1, 1, 1, 1, TimeSpan.Zero)),
                    EndOfPeriod.Create(this.now.AddYears(2)),
                    runtime));
            this.ExampleMovieReadModel = new(
                this.ExampleMovie.Id,
                this.ExampleMovie.Title,
                this.ExampleMovie.Runtime);

            this.ExampleAuditorium = Auditorium.Create(
                id: new AuditoriumId(SeedValues.ExampleAuditoriumId),
                cinemaId: this.ExampleCinema.Id,
                name: Name.Create(SeedValues.ExampleAuditoriumName),
                rows: Rows.Create(SeedValues.ExampleAuditoriumRows),
                seatsPerRow: SeatsPerRow.Create(SeedValues.ExampleAuditoriumSeatsPerRow));
            this.ExampleAuditoriumReadModel = new(
                this.ExampleAuditorium.Id,
                this.ExampleAuditorium.Name,
                this.ExampleAuditorium.CinemaId,
                this.ExampleAuditorium.Seats.Select(seat => new SeatReadModel(
                    seat.Id,
                    seat.Row,
                    seat.SeatNumber)).ToList());

            this.ExampleUserWithReservation = User.Create(
                id: new UserId(SeedValues.ExampleUserWithReservationId));
            this.ExampleUserReadModelWithReservation = new(this.ExampleUserWithReservation.Id);

            this.ExampleUserWithAdminRole = User.Create(
                id: new UserId(SeedValues.ExampleUserWithAdminRoleId));
            this.ExampleUserReadModelWithAdminRole = new(this.ExampleUserWithAdminRole.Id);

            this.ExampleUserToBeGranted = User.Create(
                id: new UserId(SeedValues.ExampleUserToBeGrantedId));
            this.ExampleUserReadModelToBeGranted = new(this.ExampleUserToBeGranted.Id);

            this.ExampleUserToBeAssignedAsViewerRole = User.Create(
                id: new UserId(SeedValues.ExampleUserToBeAssignedAsViewerRoleId));
            this.ExampleUserReadModelToBeAssignedAsViewerRole = new(this.ExampleUserToBeAssignedAsViewerRole.Id);

            this.ExampleUserToBeRevoked = User.Create(
                id: new UserId(SeedValues.ExampleUserToBeRevokedId));
            this.ExampleUserReadModelToBeRevoked = new(this.ExampleUserToBeRevoked.Id);

            this.ExampleUserToBeRemovedAsViewerRole = User.Create(
                id: new UserId(SeedValues.ExampleUserToBeRemovedAsViewerRoleId));
            this.ExampleUserReadModelToBeRemovedAsViewerRole = new(this.ExampleUserToBeRemovedAsViewerRole.Id);

            this.ExampleUserToBeAssignedScheduleShowtimePermission = User.Create(
                id: new UserId(SeedValues.ExampleUserToBeAssignedScheduleShowtimePermissionId));
            this.ExampleUserReadModelToBeAssignedScheduleShowtimePermission = new(this.ExampleUserToBeAssignedScheduleShowtimePermission.Id);

            this.ExampleUserToBeRemovedScheduleShowtimePermission = User.Create(
                id: new UserId(SeedValues.ExampleUserToBeRemovedScheduleShowtimePermissionId));
            this.ExampleUserReadModelToBeRemovedScheduleShowtimePermission = new(this.ExampleUserToBeRemovedScheduleShowtimePermission.Id);

            this.ExampleUser = User.Create(
                id: new UserId(SeedValues.ExampleUserId));
            this.ExampleUserReadModel = new(this.ExampleUser.Id);

            this.ExampleShowtime = Showtime.Schedule(
                id: new ShowtimeId(SeedValues.ExampleShowtimeId),
                movieId: this.ExampleMovie.Id,
                sessionDateOnUtc: SessionDate.Create(this.now.AddYears(1)),
                auditoriumId: this.ExampleAuditorium.Id);
            this.ExampleShowtimeReadModel = new(
                this.ExampleShowtime.Id,
                this.ExampleShowtime.SessionDateOnUtc,
                this.ExampleMovie.Id,
                this.ExampleMovie.Title,
                this.ExampleMovie.Runtime,
                this.ExampleAuditorium.Id,
                this.ExampleAuditorium.Name);

            this.ExampleReservation = new(
                id: new ReservationId(SeedValues.ExampleReservationId),
                userId: this.ExampleUserWithReservation.Id,
                seatIds: this.ExampleAuditorium.Seats.Take(3).Select(seat => seat.Id),
                reservationDateOnUtc: ReservationDate.Create(this.now.AddDays(1))); // Added 1 day to avoid conflicts with the seat reservation time in the showtime.

            // Aggregate state changes happen here, once per instance,
            // so the population methods only register what is already built.
            this.ExampleUserWithAdminRole.GrantUser(
                Scope.Create(this.ExampleTenant.Id, this.ExamplePartition.Id, this.ExampleCinema.Id),
                new[] { Role.Create(Roles.Admin) },
                []);

            this.ExampleShowtime.ReserveSeats(
                this.ExampleReservation.Id,
                this.ExampleReservation.UserId,
                this.ExampleReservation.Seats,
                this.ExampleReservation.ReservationDateOnUtc);
        }

        public Tenant SystemTenant { get; }

        public TenantReadModel SystemTenantReadModel { get; }

        public Tenant ExampleTenant { get; }

        public TenantReadModel ExampleTenantReadModel { get; }

        public Partition ExamplePartition { get; }

        public PartitionReadModel ExamplePartitionReadModel { get; }

        public Cinema ExampleCinema { get; }

        public CinemaReadModel ExampleCinemaReadModel { get; }

        public Movie ExampleMovie { get; }

        public MovieReadModel ExampleMovieReadModel { get; }

        public Auditorium ExampleAuditorium { get; }

        public AuditoriumReadModel ExampleAuditoriumReadModel { get; }

        public User ExampleUserWithReservation { get; }

        public UserReadModel ExampleUserReadModelWithReservation { get; }

        public User ExampleUserWithAdminRole { get; }

        public UserReadModel ExampleUserReadModelWithAdminRole { get; }

        public User ExampleUserToBeGranted { get; }

        public UserReadModel ExampleUserReadModelToBeGranted { get; }

        public User ExampleUserToBeAssignedAsViewerRole { get; }

        public UserReadModel ExampleUserReadModelToBeAssignedAsViewerRole { get; }

        public User ExampleUserToBeRevoked { get; }

        public UserReadModel ExampleUserReadModelToBeRevoked { get; }

        public User ExampleUserToBeRemovedAsViewerRole { get; }

        public UserReadModel ExampleUserReadModelToBeRemovedAsViewerRole { get; }

        public User ExampleUserToBeAssignedScheduleShowtimePermission { get; }

        public UserReadModel ExampleUserReadModelToBeAssignedScheduleShowtimePermission { get; }

        public User ExampleUserToBeRemovedScheduleShowtimePermission { get; }

        public UserReadModel ExampleUserReadModelToBeRemovedScheduleShowtimePermission { get; }

        public User ExampleUser { get; }

        public UserReadModel ExampleUserReadModel { get; }

        public Showtime ExampleShowtime { get; }

        public ShowtimeReadModel ExampleShowtimeReadModel { get; }

        public Reservation ExampleReservation { get; }

        public ReadOnlyCollection<AvailableSeatReadModel> ExampleAvailableSeatsReadModel =>
            this.ExampleAuditorium.Seats
                .Where(seat => !this.ExampleReservation.Seats.Contains(seat.Id))
                .Select(seat => new AvailableSeatReadModel(
                    id: Guid.CreateVersion7(),
                    seatId: seat.Id,
                    row: seat.Row,
                    seatNumber: seat.SeatNumber,
                    showtimeId: this.ExampleShowtime.Id,
                    auditoriumId: this.ExampleAuditorium.Id,
                    auditoriumName: this.ExampleAuditorium.Name))
                .ToList()
                .AsReadOnly();

        public ReservationReadModel ExampleReservationReadModel() =>
            new(
                id: this.ExampleReservation.Id,
                userId: this.ExampleUserWithReservation.Id,
                showtimeId: this.ExampleShowtime.Id,
                sessionDateOnUtc: this.ExampleShowtime.SessionDateOnUtc,
                auditoriumName: this.ExampleAuditorium.Name,
                movieTitle: this.ExampleMovie.Title,
                seats: this.ExampleAuditorium.Seats.Take(3).Select(seat => new SeatReadModel(
                    seat.Id,
                    seat.Row,
                    seat.SeatNumber)).ToList(),
                isPurchased: false,
                createdTimeOnUtc: this.now);

        public UserAuthorizationReadModel ExampleUserAuthorizationReadModelWithAdminRole()
        {
            var userAuthorization = new UserAuthorizationReadModel(this.ExampleUserWithAdminRole.Id)
            {
                UserId = this.ExampleUserWithAdminRole.Id,
                TenantId = this.ExampleTenant.Id,
                PartitionId = this.ExamplePartition.Id,
                DomainId = this.ExampleCinema.Id,
                Roles = new List<RoleReadModel>
                {
                    new RoleReadModel(Guid.CreateVersion7(), Roles.Admin),
                },
            };

            return userAuthorization;
        }

        public void PopulateReadModelTestData(ReservationReadModelContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            var tenantId = this.ExampleTenant.Id;
            var partitionId = this.ExamplePartition.Id;
            var domainId = this.ExampleCinema.Id;

            // Local factory: each entity needs its own instance of ScopeInfo.
            // EF Core cannot resolve the same owned type reference
            // for multiple owner entities within the same SaveChanges.
            ScopeInfo NewScope() => new(tenantId, partitionId, domainId);

            this.ExampleMovieReadModel.Scope = NewScope();
            context.Movies.Add(this.ExampleMovieReadModel);

            this.ExampleAuditoriumReadModel.Scope = NewScope();
            context.Auditoriums.Add(this.ExampleAuditoriumReadModel);

            this.ExampleUserReadModelWithAdminRole.Scope = NewScope();
            context.Users.Add(this.ExampleUserReadModelWithAdminRole);

            this.ExampleUserReadModelWithReservation.Scope = NewScope();
            context.Users.Add(this.ExampleUserReadModelWithReservation);

            this.ExampleUserReadModel.Scope = NewScope();
            context.Users.Add(this.ExampleUserReadModel);

            var exampleUserAuthorizationReadModelWithAdminRole = this.ExampleUserAuthorizationReadModelWithAdminRole();
            exampleUserAuthorizationReadModelWithAdminRole.Scope = NewScope();
            context.UsersAuthorizations.Add(exampleUserAuthorizationReadModelWithAdminRole);

            this.SystemTenantReadModel.Scope = NewScope();
            context.Tenants.Add(this.SystemTenantReadModel);

            this.ExampleTenantReadModel.Scope = NewScope();
            context.Tenants.Add(this.ExampleTenantReadModel);

            this.ExamplePartitionReadModel.Scope = NewScope();
            context.Partitions.Add(this.ExamplePartitionReadModel);

            this.ExampleCinemaReadModel.Scope = NewScope();
            context.Cinemas.Add(this.ExampleCinemaReadModel);

            this.ExampleShowtimeReadModel.Scope = NewScope();
            context.Showtimes.Add(this.ExampleShowtimeReadModel);

            var exampleReservationReadModel = this.ExampleReservationReadModel();
            exampleReservationReadModel.Scope = NewScope();
            context.Reservations.Add(exampleReservationReadModel);

            var availableSeats = this.ExampleAvailableSeatsReadModel;
            foreach (var seat in availableSeats)
            {
                seat.Scope = NewScope();
            }

            context.AvailableSeats.AddRange(availableSeats);

            context.SaveChanges();
        }

        public async Task PopulateBusinessModelTestDataAsync(
            IEventStore eventStore,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(eventStore, nameof(eventStore));

            var hasExistingData = await this.HasExistingDataAsync(eventStore, cancellationToken);
            if (hasExistingData)
            {
                return;
            }

            eventStore.AppendChanges<Movie, MovieId>(this.ExampleMovie);
            eventStore.AppendChanges<Auditorium, AuditoriumId>(this.ExampleAuditorium);
            eventStore.AppendChanges<Partition, PartitionId>(this.ExamplePartition);
            eventStore.AppendChanges<Tenant, TenantId>(this.SystemTenant);
            eventStore.AppendChanges<Tenant, TenantId>(this.ExampleTenant);
            eventStore.AppendChanges<Cinema, CinemaId>(this.ExampleCinema);

            eventStore.AppendChanges<User, UserId>(this.ExampleUserToBeAssignedAsViewerRole);
            eventStore.AppendChanges<User, UserId>(this.ExampleUserToBeRevoked);
            eventStore.AppendChanges<User, UserId>(this.ExampleUserToBeRemovedAsViewerRole);
            eventStore.AppendChanges<User, UserId>(this.ExampleUserToBeAssignedScheduleShowtimePermission);
            eventStore.AppendChanges<User, UserId>(this.ExampleUserToBeRemovedScheduleShowtimePermission);
            eventStore.AppendChanges<User, UserId>(this.ExampleUserToBeGranted);
            eventStore.AppendChanges<User, UserId>(this.ExampleUserWithAdminRole);
            eventStore.AppendChanges<User, UserId>(this.ExampleUserWithReservation);
            eventStore.AppendChanges<User, UserId>(this.ExampleUser);

            eventStore.AppendChanges<Showtime, ShowtimeId>(this.ExampleShowtime);

            await eventStore.SaveChangesAsync(cancellationToken);
        }

        private async Task<bool> HasExistingDataAsync(IEventStore eventStore, CancellationToken cancellationToken)
        {
            var existingMovie = await eventStore.LoadAggregateAsync<Movie, MovieId>(
                this.ExampleMovie.Id,
                cancellationToken);

            var existingAuditorium = await eventStore.LoadAggregateAsync<Auditorium, AuditoriumId>(
                this.ExampleAuditorium.Id,
                cancellationToken);

            var existingUserWithAdminRole = await eventStore.LoadAggregateAsync<User, UserId>(
                this.ExampleUserWithAdminRole.Id,
                cancellationToken);

            var existingUser = await eventStore.LoadAggregateAsync<User, UserId>(
                this.ExampleUser.Id,
                cancellationToken);

            var existingPartition = await eventStore.LoadAggregateAsync<Partition, PartitionId>(
                this.ExamplePartition.Id,
                cancellationToken);

            var existingTenant = await eventStore.LoadAggregateAsync<Tenant, TenantId>(
                this.ExampleTenant.Id,
                cancellationToken);

            var existingCinema = await eventStore.LoadAggregateAsync<Cinema, CinemaId>(
                this.ExampleCinema.Id,
                cancellationToken);

            var existingShowtime = await eventStore.LoadAggregateAsync<Showtime, ShowtimeId>(
                this.ExampleShowtime.Id,
                cancellationToken);

            return existingMovie != null
                || existingAuditorium != null
                || existingUserWithAdminRole != null
                || existingUser != null
                || existingPartition != null
                || existingTenant != null
                || existingCinema != null
                || existingShowtime != null;
        }
    }
}
