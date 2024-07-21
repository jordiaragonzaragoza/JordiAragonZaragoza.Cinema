namespace JordiAragon.Cinema.Reservation.Auditorium.Domain
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain.Events;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;
    using JordiAragon.SharedKernel.Domain.Exceptions;

    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class Auditorium : BaseAggregateRoot<AuditoriumId, Guid>
    {
        private readonly List<ShowtimeId> activeShowtimes = new();
        private List<Seat> seats = new();

        // Required by EF
        private Auditorium()
        {
        }

        // TODO: It belongs to the cinema manager bounded context.
        public string Name { get; private set; } = string.Empty;

        public Rows Rows { get; private set; } = default!;

        public SeatsPerRow SeatsPerRow { get; private set; } = default!;

        public IEnumerable<ShowtimeId> ActiveShowtimes => this.activeShowtimes.AsReadOnly();

        public IEnumerable<Seat> Seats => this.seats.AsReadOnly();

        public static Auditorium Create(
            AuditoriumId id,
            string name,
            Rows rows,
            SeatsPerRow seatsPerRow)
        {
            var auditorium = new Auditorium();

            auditorium.Apply(new AuditoriumCreatedEvent(id, name, rows, seatsPerRow));

            return auditorium;
        }

        public void Remove()
            => this.Apply(new AuditoriumRemovedEvent(this.Id));

        public void AddActiveShowtime(ShowtimeId showtimeId)
            => this.Apply(new ActiveShowtimeAddedEvent(this.Id, showtimeId));

        public void RemoveActiveShowtime(ShowtimeId showtimeId)
            => this.Apply(new ActiveShowtimeRemovedEvent(this.Id, showtimeId));

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case AuditoriumCreatedEvent @event:
                    this.Applier(@event);
                    break;

                case AuditoriumRemovedEvent:
                    break;

                case ActiveShowtimeAddedEvent @event:
                    this.activeShowtimes.Add(ShowtimeId.Create(@event.ShowtimeId));
                    break;

                case ActiveShowtimeRemovedEvent @event:
                    this.Applier(@event);
                    break;
            }
        }

        protected override void EnsureValidState()
        {
            try
            {
                Guard.Against.Null(this.Id, nameof(this.Id));
                Guard.Against.NullOrWhiteSpace(this.Name, nameof(this.Name));
                Guard.Against.Null(this.Rows, nameof(this.Rows));
                Guard.Against.Null(this.SeatsPerRow, nameof(this.SeatsPerRow));
            }
            catch (Exception exception)
            {
                throw new InvalidAggregateStateException<Auditorium, AuditoriumId>(this, exception.Message);
            }
        }

        private static List<Seat> GenerateSeats(ushort rows, ushort seatsPerRow)
        {
            var generatedSeats = new List<Seat>();
            for (short row = 1; row <= rows; row++)
            {
                for (short seat = 1; seat <= seatsPerRow; seat++)
                {
                    generatedSeats.Add(Seat.Create(SeatId.Create(Guid.NewGuid()), row, seat));
                }
            }

            return generatedSeats;
        }

        private void Applier(AuditoriumCreatedEvent @event)
        {
            this.Id = AuditoriumId.Create(@event.AggregateId);
            this.Name = @event.Name;
            this.Rows = Rows.Create(@event.Rows);
            this.SeatsPerRow = SeatsPerRow.Create(@event.SeatsPerRow);
            this.seats = GenerateSeats(this.Rows, this.SeatsPerRow);
        }

        private void Applier(ActiveShowtimeRemovedEvent @event)
        {
            var isRemoved = this.activeShowtimes.Remove(ShowtimeId.Create(@event.ShowtimeId));
            if (!isRemoved)
            {
                throw new NotFoundException(nameof(Showtime), @event.ShowtimeId.ToString());
            }
        }
    }
}