namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels
{
    using System;
    using System.Collections.Generic;
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
            this.Id = id;
            this.UserId = userId;
            this.ShowtimeId = showtimeId;
            this.SessionDateOnUtc = sessionDateOnUtc;
            this.AuditoriumName = auditoriumName;
            this.MovieTitle = movieTitle;
            this.Seats = seats;
            this.IsPurchased = isPurchased;
            this.CreatedTimeOnUtc = createdTimeOnUtc;
        }

        // Required by EF.
        private TicketReadModel()
        {
        }

        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public Guid ShowtimeId { get; private set; }

        public DateTimeOffset SessionDateOnUtc { get; private set; }

        public string AuditoriumName { get; private set; }

        public string MovieTitle { get; private set; }

        public IEnumerable<SeatReadModel> Seats { get; private set; }

        public bool IsPurchased { get; set; }

        public DateTimeOffset CreatedTimeOnUtc { get; private set; } // TODO: Review if its required.
    }
}