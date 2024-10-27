namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels
{
    using System;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class SeatReadModel : IReadModel
    {
        public SeatReadModel(
            Guid id,
            ushort row,
            ushort seatNumber)
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

        public ushort Row { get; private set; }

        public ushort SeatNumber { get; private set; }
    }
}