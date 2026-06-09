namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Events;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Rules;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;

    public sealed class Auditorium : BaseEventSourcedAggregateRoot<AuditoriumId, Guid>
    {
        private readonly List<ShowtimeId> activeShowtimes = new();
        private List<Seat> seats = new();

        // Required by EF
        private Auditorium()
        {
        }

        // TODO: It belongs to the cinema manager bounded context. Not required.
        public Name Name { get; private set; } = default!;

        public Rows Rows { get; private set; } = default!;

        public SeatsPerRow SeatsPerRow { get; private set; } = default!;

        public IEnumerable<ShowtimeId> ActiveShowtimes => this.activeShowtimes.AsReadOnly();

        public IEnumerable<Seat> Seats => this.seats.AsReadOnly();

        // TODO: Review. Will API changed passing the seats as a parameter?.
        public static Auditorium Create(
            AuditoriumId id,
            Name name,
            Rows rows,
            SeatsPerRow seatsPerRow)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            ArgumentNullException.ThrowIfNull(rows, nameof(rows));
            ArgumentNullException.ThrowIfNull(seatsPerRow, nameof(seatsPerRow));

            var auditorium = new Auditorium();

            var (seatIds, seatRows, seatNumbers) = GenerateSeats(rows, seatsPerRow);

            auditorium.Apply(new AuditoriumCreatedEvent(
                id,
                name,
                rows,
                seatsPerRow,
                seatIds.AsReadOnly(),
                seatRows.AsReadOnly(),
                seatNumbers.AsReadOnly()));

            return auditorium;
        }

        public void Remove()
        {
            // An Auditorium cannot be deleted if it has active showtimes.
            // Removing an auditorium is only possible when the system has reached a consistent state with respect to its showtimes.
            CheckRule(new OnlyAuditoriumsWithoutActiveShowtimesCanBeRemovedRule(this.activeShowtimes.AsReadOnly()));

            this.Apply(new AuditoriumRemovedEvent(this.Id));
        }

        public void ScheduleShowtime(ShowtimeId showtimeId)
        {
            ArgumentNullException.ThrowIfNull(showtimeId, nameof(showtimeId));

            CheckRule(new ShowtimeMustNotBeAlreadyRegisteredRule(this.activeShowtimes.AsReadOnly(), showtimeId));

            this.Apply(new ShowtimeScheduledEvent(this.Id, showtimeId));
        }

        public void CancelShowtime(ShowtimeId showtimeId)
        {
            ArgumentNullException.ThrowIfNull(showtimeId, nameof(showtimeId));

            CheckRule(new ShowtimeHasToBeAlreadyRegisteredRule(this.activeShowtimes.AsReadOnly(), showtimeId));

            this.Apply(new ShowtimeCanceledEvent(this.Id, showtimeId));
        }

        public void EndShowtime(ShowtimeId showtimeId)
        {
            ArgumentNullException.ThrowIfNull(showtimeId, nameof(showtimeId));

            CheckRule(new ShowtimeHasToBeAlreadyRegisteredRule(this.activeShowtimes.AsReadOnly(), showtimeId));

            this.Apply(new ShowtimeEndedEvent(this.Id, showtimeId));
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

                case ShowtimeScheduledEvent @event:
                    this.activeShowtimes.Add(new ShowtimeId(@event.ShowtimeId));
                    break;

                case ShowtimeCanceledEvent @event:
                    this.activeShowtimes.Remove(new ShowtimeId(@event.ShowtimeId));
                    break;

                case ShowtimeEndedEvent @event:
                    this.activeShowtimes.Remove(new ShowtimeId(@event.ShowtimeId));
                    break;

                default:
                    throw new EventCannotBeAppliedToAggregateException<Auditorium, AuditoriumId>(this, domainEvent);
            }
        }

        protected override void EnsureValidState()
        {
            // Not required validation post apply events. This is a deterministic aggregate.
            // All the validations are done on public methods.
        }

        private static (List<Guid> SeatIds, List<ushort> SeatRows, List<ushort> SeatNumbers) GenerateSeats(
            ushort rows,
            ushort seatsPerRow)
        {
            var seatIds = new List<Guid>();
            var seatRows = new List<ushort>();
            var seatNumbers = new List<ushort>();

            for (ushort row = 1; row <= rows; row++)
            {
                for (ushort seatNumber = 1; seatNumber <= seatsPerRow; seatNumber++)
                {
                    seatIds.Add(Guid.CreateVersion7());
                    seatRows.Add(row);
                    seatNumbers.Add(seatNumber);
                }
            }

            return (seatIds, seatRows, seatNumbers);
        }

        private void Applier(AuditoriumCreatedEvent @event)
        {
            this.Id = new AuditoriumId(@event.AggregateId);
            this.Name = new Name(@event.Name);
            this.Rows = new Rows(@event.Rows);
            this.SeatsPerRow = new SeatsPerRow(@event.SeatsPerRow);

            this.seats = @event.SeatIds
                .Select((seatId, index) => new Seat(
                    new SeatId(seatId),
                    new Row(@event.SeatRows[index]),
                    new SeatNumber(@event.SeatNumbers[index])))
                .ToList();
        }
    }
}