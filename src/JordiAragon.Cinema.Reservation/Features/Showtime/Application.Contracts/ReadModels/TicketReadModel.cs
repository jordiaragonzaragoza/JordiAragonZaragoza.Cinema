namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class TicketReadModel : IReadModel
    {
        public TicketReadModel(
            Guid id,
            Guid userId,
            Guid showtimeId,
            DateTimeOffset sessionDateOnUtc,
            string auditoriumName,
            string movieTitle,
            IEnumerable<SeatReadModel> seats,
            bool isPurchased,
            DateTimeOffset createdTimeOnUtc)
        {
            this.Id = Guard.Against.Default(id, nameof(id));
            this.UserId = Guard.Against.Default(userId, nameof(userId));
            this.ShowtimeId = Guard.Against.Default(showtimeId, nameof(showtimeId));
            this.SessionDateOnUtc = Guard.Against.Default(sessionDateOnUtc, nameof(sessionDateOnUtc));
            this.AuditoriumName = Guard.Against.NullOrWhiteSpace(auditoriumName, nameof(auditoriumName));
            this.MovieTitle = Guard.Against.NullOrWhiteSpace(movieTitle, nameof(movieTitle));
            this.Seats = Guard.Against.NullOrEmpty(seats, nameof(seats));
            this.IsPurchased = isPurchased;
            this.CreatedTimeOnUtc = Guard.Against.Default(createdTimeOnUtc, nameof(createdTimeOnUtc));
        }

        // Required by EF.
        private TicketReadModel()
        {
        }

        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public Guid ShowtimeId { get; private set; }

        public DateTimeOffset SessionDateOnUtc { get; private set; }

        public string AuditoriumName { get; private set; } = default!;

        public string MovieTitle { get; private set; } = default!;

        public IEnumerable<SeatReadModel> Seats { get; private set; } = new List<SeatReadModel>();

        public bool IsPurchased { get; set; }

        public DateTimeOffset CreatedTimeOnUtc { get; private set; } // TODO: Review if its required.
    }
}