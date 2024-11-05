namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Events;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class Auditorium : BaseAggregateRoot<AuditoriumId, Guid>
    {
        private readonly List<ShowtimeId> activeShowtimes = new();
        private List<Seat> seats = new();

        // Required by EF
        private Auditorium()
        {
        }

        // TODO: It belongs to the cinema manager bounded context.
        public string Name { get; private set; } = default!;

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
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            ArgumentNullException.ThrowIfNull(rows, nameof(rows));
            ArgumentNullException.ThrowIfNull(seatsPerRow, nameof(seatsPerRow));

            var auditorium = new Auditorium();

            auditorium.Apply(new AuditoriumCreatedEvent(id, name, rows, seatsPerRow));

            return auditorium;
        }

        public void Remove()
            => this.Apply(new AuditoriumRemovedEvent(this.Id));

        public void AddActiveShowtime(ShowtimeId showtimeId)
        {
            ArgumentNullException.ThrowIfNull(showtimeId, nameof(showtimeId));

            this.Apply(new ActiveShowtimeAddedEvent(this.Id, showtimeId));
        }

        public void RemoveActiveShowtime(ShowtimeId showtimeId)
        {
            ArgumentNullException.ThrowIfNull(showtimeId, nameof(showtimeId));

            this.Apply(new ActiveShowtimeRemovedEvent(this.Id, showtimeId));
        }

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
                    this.activeShowtimes.Add(new ShowtimeId(@event.ShowtimeId));
                    break;

                case ActiveShowtimeRemovedEvent @event:
                    this.Applier(@event);
                    break;

                default:
                    throw new EventCannotBeAppliedToAggregateException<Auditorium, AuditoriumId>(this, domainEvent);
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
            for (ushort row = 1; row <= rows; row++)
            {
                for (ushort seatNumber = 1; seatNumber <= seatsPerRow; seatNumber++)
                {
                    generatedSeats.Add(new Seat(new SeatId(Guid.NewGuid()), new Row(row), new SeatNumber(seatNumber)));
                }
            }

            return generatedSeats;
        }

        private void Applier(AuditoriumCreatedEvent @event)
        {
            this.Id = new AuditoriumId(@event.AggregateId);
            this.Name = @event.Name;
            this.Rows = new Rows(@event.Rows);
            this.SeatsPerRow = new SeatsPerRow(@event.SeatsPerRow);
            this.seats = GenerateSeats(this.Rows, this.SeatsPerRow);
        }

        private void Applier(ActiveShowtimeRemovedEvent @event)
        {
            var isRemoved = this.activeShowtimes.Remove(new ShowtimeId(@event.ShowtimeId));
            if (!isRemoved)
            {
                throw new NotFoundException(nameof(Showtime), @event.ShowtimeId.ToString());
            }
        }
    }
}