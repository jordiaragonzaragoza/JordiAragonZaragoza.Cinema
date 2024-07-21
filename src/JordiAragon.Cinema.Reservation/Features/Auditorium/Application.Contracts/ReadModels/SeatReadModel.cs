namespace JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class SeatReadModel : IReadModel
    {
        public SeatReadModel(
            Guid id,
            short row,
            short seatNumber)
        {
            this.Id = Guard.Against.Default(id, nameof(id));
            this.Row = Guard.Against.Default(row, nameof(row));
            this.SeatNumber = Guard.Against.Default(seatNumber, nameof(seatNumber));
        }

        // Required by EF.
        private SeatReadModel()
        {
        }

        public Guid Id { get; private set; }

        public short Row { get; private set; } // TODO: Use ushort

        public short SeatNumber { get; private set; } // TODO: Use ushort
    }
}