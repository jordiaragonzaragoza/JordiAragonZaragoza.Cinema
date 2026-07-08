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
            Guid cinemaId,
            IEnumerable<SeatReadModel> seats)
        {
            this.Id = Guard.Against.Default(id, nameof(id));
            this.Name = Guard.Against.Default(name, nameof(name));
            this.CinemaId = Guard.Against.Default(cinemaId, nameof(cinemaId));
            this.Seats = Guard.Against.NullOrEmpty(seats, nameof(seats));
        }

        // Required by EF.
        private AuditoriumReadModel()
        {
        }

        public string Name { get; private set; } = default!;

        public Guid CinemaId { get; private set; }

        public IEnumerable<SeatReadModel> Seats { get; private set; } = new List<SeatReadModel>();
    }
}