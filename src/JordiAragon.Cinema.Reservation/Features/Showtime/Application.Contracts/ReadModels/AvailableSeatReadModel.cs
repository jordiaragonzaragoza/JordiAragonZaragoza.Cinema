namespace JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class AvailableSeatReadModel : IReadModel
    {
        public AvailableSeatReadModel(
            Guid id,
            Guid seatId,
            short row,
            short seatNumber,
            Guid showtimeId,
            Guid auditoriumId,
            string auditoriumName)
        {
            this.Id = Guard.Against.Default(id, nameof(id));
            this.SeatId = Guard.Against.Default(seatId, nameof(seatId));
            this.Row = Guard.Against.Default(row, nameof(row));
            this.SeatNumber = Guard.Against.Default(seatNumber, nameof(seatNumber));
            this.ShowtimeId = Guard.Against.Default(showtimeId, nameof(showtimeId));
            this.AuditoriumId = Guard.Against.Default(auditoriumId, nameof(auditoriumId));
            this.AuditoriumName = Guard.Against.NullOrWhiteSpace(auditoriumName, nameof(auditoriumName));
        }

        // Required by EF.
        private AvailableSeatReadModel()
        {
        }

        public Guid Id { get; private set; }

        public Guid SeatId { get; private set; }

        public short Row { get; private set; } // TODO: Use ushort

        public short SeatNumber { get; private set; } // TODO: Use ushort

        public Guid ShowtimeId { get; private set; }

        public Guid AuditoriumId { get; private set; }

        public string AuditoriumName { get; private set; } = string.Empty;
    }
}