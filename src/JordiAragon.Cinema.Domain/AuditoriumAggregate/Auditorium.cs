namespace JordiAragon.Cinema.Domain.AuditoriumAggregate
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Events;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;
    using JordiAragon.SharedKernel.Domain.Exceptions;

    using NotFoundException = JordiAragon.SharedKernel.Domain.Exceptions.NotFoundException;

    public class Auditorium : BaseAggregateRoot<AuditoriumId, Guid>
    {
        private readonly List<ShowtimeId> showtimes = new();
        private List<Seat> seats;

        // Required by EF
        private Auditorium()
        {
        }

        public short Rows { get; private set; }

        public short SeatsPerRow { get; private set; }

        public IEnumerable<ShowtimeId> Showtimes => this.showtimes.AsReadOnly();

        public IEnumerable<Seat> Seats => this.seats.AsReadOnly();

        public static Auditorium Create(
            AuditoriumId id,
            short rows,
            short seatsPerRow)
        {
            var auditorium = new Auditorium();

            auditorium.Apply(new AuditoriumCreatedEvent(id, rows, seatsPerRow));

            return auditorium;
        }

        public void AddShowtime(ShowtimeId showtimeId)
            => this.Apply(new ShowtimeAddedEvent(this.Id, showtimeId));

        public void RemoveShowtime(ShowtimeId showtimeId)
            => this.Apply(new ShowtimeRemovedEvent(this.Id, showtimeId));

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case AuditoriumCreatedEvent @event:
                    this.Applier(@event);
                    break;

                case ShowtimeAddedEvent @event:
                    this.showtimes.Add(ShowtimeId.Create(@event.ShowtimeId));
                    break;

                case ShowtimeRemovedEvent @event:
                    this.Applier(@event);
                    break;
            }
        }

        protected override void EnsureValidState()
        {
            try
            {
                Guard.Against.Null(this.Id, nameof(this.Id));
                Guard.Against.NegativeOrZero(this.Rows, nameof(this.Rows));
                Guard.Against.NegativeOrZero(this.SeatsPerRow, nameof(this.SeatsPerRow));
            }
            catch (Exception exception)
            {
                throw new InvalidAggregateStateException(this, exception.Message);
            }
        }

        private static List<Seat> GenerateSeats(short rows, short seatsPerRow)
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
            this.Rows = @event.Rows;
            this.SeatsPerRow = @event.SeatsPerRow;
            this.seats = GenerateSeats(this.Rows, this.SeatsPerRow);
        }

        private void Applier(ShowtimeRemovedEvent @event)
        {
            var isRemoved = this.showtimes.Remove(ShowtimeId.Create(@event.ShowtimeId));
            if (!isRemoved)
            {
                throw new NotFoundException(nameof(Showtime), @event.ShowtimeId.ToString());
            }
        }
    }
}