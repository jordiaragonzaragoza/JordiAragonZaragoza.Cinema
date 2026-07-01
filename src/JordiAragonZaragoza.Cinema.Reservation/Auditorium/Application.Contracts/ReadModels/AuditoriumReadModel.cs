namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.SharedKernel.Application.ReadModels;

    public sealed class AuditoriumReadModel : BaseReadModel
    {
        public AuditoriumReadModel(
            Guid id,
            string name,
            IEnumerable<SeatReadModel> seats)
        {
            this.Id = Guard.Against.Default(id, nameof(id));
            this.Name = Guard.Against.Default(name, nameof(name));
            this.Seats = Guard.Against.NullOrEmpty(seats, nameof(seats));
        }

        // Required by EF.
        private AuditoriumReadModel()
        {
        }

        public string Name { get; private set; } = default!;

        public IEnumerable<SeatReadModel> Seats { get; private set; } = new List<SeatReadModel>();
    }
}