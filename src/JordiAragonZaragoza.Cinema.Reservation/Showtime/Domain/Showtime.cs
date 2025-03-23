namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Events;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Rules;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class Showtime : BaseAggregateRoot<ShowtimeId, Guid>
    {
        private readonly List<Reservation> reservations = new();

        // Required by EF.
        private Showtime()
        {
        }

        public MovieId MovieId { get; private set; } = default!;

        public SessionDate SessionDateOnUtc { get; private set; } = default!;

        public AuditoriumId AuditoriumId { get; private set; } = default!;

        public bool IsEnded { get; private set; } // TODO: Review. Probably this property is not required.

        public IEnumerable<Reservation> Reservations => this.reservations.AsReadOnly();

        public static Showtime Schedule(
            ShowtimeId id,
            MovieId movieId,
            SessionDate sessionDateOnUtc,
            AuditoriumId auditoriumId)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            ArgumentNullException.ThrowIfNull(movieId, nameof(movieId));
            ArgumentNullException.ThrowIfNull(sessionDateOnUtc, nameof(sessionDateOnUtc));
            ArgumentNullException.ThrowIfNull(auditoriumId, nameof(auditoriumId));

            var showtime = new Showtime();

            showtime.Apply(new ShowtimeScheduledEvent(id, movieId, sessionDateOnUtc, auditoriumId));

            return showtime;
        }

        public void Cancel()
            => this.Apply(new ShowtimeCanceledEvent(this.Id, this.AuditoriumId, this.MovieId));

        public void End()
            => this.Apply(new ShowtimeEndedEvent(this.Id, this.AuditoriumId, this.MovieId));

        public Reservation ReserveSeats(ReservationId id, UserId userId, IEnumerable<SeatId> seatIds, ReservationDate reservationDateOnUtc)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));
            Guard.Against.NullOrEmpty(seatIds, nameof(seatIds));
            ArgumentNullException.ThrowIfNull(reservationDateOnUtc, nameof(reservationDateOnUtc));

            this.Apply(new ReservedSeatsEvent(this.Id, id, userId, seatIds.Select(x => x.Value), reservationDateOnUtc));

            return this.reservations[^1];
        }

        public void PurchaseReservation(ReservationId reservationId)
        {
            ArgumentNullException.ThrowIfNull(reservationId, nameof(reservationId));

            var reservation = this.Reservations.FirstOrDefault(reservation => reservation.Id == reservationId)
                         ?? throw new NotFoundException(nameof(Reservation), reservationId.Value);

            CheckRule(new OnlyPossibleToPurchaseOncePerReservationRule(reservation));

            this.Apply(new PurchasedReservationEvent(this.Id, reservationId));
        }

        public void ExpireReservedSeats(ReservationId reservationToRemove)
        {
            ArgumentNullException.ThrowIfNull(reservationToRemove, nameof(reservationToRemove));

            var reservation = this.Reservations.FirstOrDefault(item => item.Id == reservationToRemove)
                ?? throw new NotFoundException(nameof(Reservation), reservationToRemove.Value);

            var seatIds = reservation.Seats.Select(seatId => seatId.Value);

            this.Apply(new ExpiredReservedSeatsEvent(this.Id, reservationToRemove, seatIds));
        }

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case ShowtimeScheduledEvent @event:
                    this.Applier(@event);
                    break;

                case ShowtimeCanceledEvent:
                    break;

                case ShowtimeEndedEvent:
                    this.Applier();
                    break;

                case ReservedSeatsEvent @event:
                    this.Applier(@event);
                    break;

                case PurchasedReservationEvent @event:
                    this.Applier(@event);
                    break;

                case ExpiredReservedSeatsEvent @event:
                    this.Applier(@event);
                    break;

                default:
                    throw new EventCannotBeAppliedToAggregateException<Showtime, ShowtimeId>(this, domainEvent);
            }
        }

        protected override void EnsureValidState()
        {
            // Not required validation post apply events. This is a deterministic aggregate.
            // All the validations are done on public methods.
        }

        private void Applier(ShowtimeScheduledEvent @event)
        {
            this.Id = new ShowtimeId(@event.AggregateId);
            this.MovieId = new MovieId(@event.MovieId);
            this.SessionDateOnUtc = new SessionDate(@event.SessionDateOnUtc);
            this.AuditoriumId = new AuditoriumId(@event.AuditoriumId);
        }

        private void Applier()
            => this.IsEnded = true;

        private void Applier(ReservedSeatsEvent @event)
        {
            var seatIds = @event.SeatIds.Select(i => new SeatId(i));

            var newReservation = new Reservation(
                 new ReservationId(@event.ReservationId),
                 new UserId(@event.UserId),
                 seatIds,
                 new ReservationDate(@event.CreatedTimeOnUtc));

            this.reservations.Add(newReservation);
        }

        private void Applier(PurchasedReservationEvent @event)
        {
            var reservationId = new ReservationId(@event.ReservationId);

            var reservation = this.Reservations.FirstOrDefault(reservation => reservation.Id == reservationId);

            reservation?.MarkAsPurchased();
        }

        private void Applier(ExpiredReservedSeatsEvent @event)
        {
            var reservationToRemove = new ReservationId(@event.ReservationId);

            var existingReservation = this.Reservations.FirstOrDefault(item => item.Id == reservationToRemove);

            this.reservations.Remove(existingReservation!);
        }
    }
}