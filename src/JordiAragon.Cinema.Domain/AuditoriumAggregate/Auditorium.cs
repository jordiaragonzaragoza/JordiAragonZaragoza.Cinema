namespace JordiAragon.Cinema.Domain.AuditoriumAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Events;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;

    public class Auditorium : BaseAuditableEntity<AuditoriumId>, IAggregateRoot
    {
        private readonly List<Showtime> showtimes = new();
        private readonly List<Seat> seats;

        public Auditorium(
            AuditoriumId id,
            IEnumerable<Seat> seats)
            : this(id)
        {
            this.seats = Guard.Against.NullOrEmpty(seats, nameof(seats)).ToList();
        }

        private Auditorium(
            AuditoriumId id)
            : base(id)
        {
        }

        public IEnumerable<Showtime> Showtimes => this.showtimes.AsReadOnly();

        public IEnumerable<Seat> Seats => this.seats.AsReadOnly();

        public static Auditorium Create(
            AuditoriumId id,
            IEnumerable<Seat> seats)
        {
            return new Auditorium(id, seats);
        }

        public Showtime AddShowtime(ShowtimeId id, MovieId movieId, DateTime sessionDateOnUtc)
        {
            var newShowtime = Showtime.Create(
                id,
                movieId,
                sessionDateOnUtc,
                this);

            this.showtimes.Add(newShowtime);

            var newItemAddedEvent = new ShowtimeAddedEvent(newShowtime, this);
            this.RegisterDomainEvent(newItemAddedEvent);

            return newShowtime;
        }
    }
}