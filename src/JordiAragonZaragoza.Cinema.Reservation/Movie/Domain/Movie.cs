namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Events;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules;

    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;

    using NotFoundException = JordiAragonZaragoza.SharedKernel.Domain.Exceptions.NotFoundException;

    public sealed class Movie : BaseEventSourcedAggregateRoot<MovieId, Guid>
    {
        private readonly List<ShowtimeId> activeShowtimes = new();

        // Required by EF.
        private Movie()
        {
        }

        // TODO: It will be removed on view model composition implementation.
        // It belongs to the catalog bounded context but title is inmutable.
        public Title Title { get; private set; } = default!;

        public Runtime Runtime { get; private set; } = default!;

        public ExhibitionPeriod ExhibitionPeriod { get; private set; } = default!;

        public IEnumerable<ShowtimeId> ActiveShowtimes => this.activeShowtimes.AsReadOnly();

        public static Movie Add(
            MovieId id,
            Title title,
            Runtime runtime,
            ExhibitionPeriod exhibitionPeriod)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            ArgumentNullException.ThrowIfNull(title, nameof(title));
            ArgumentNullException.ThrowIfNull(runtime, nameof(runtime));
            ArgumentNullException.ThrowIfNull(exhibitionPeriod, nameof(exhibitionPeriod));

            var movie = new Movie();

            movie.Apply(new MovieAddedEvent(id, title, runtime, exhibitionPeriod.StartingPeriodOnUtc, exhibitionPeriod.EndOfPeriodOnUtc));

            return movie;
        }

        public void Remove()
        {
            // An Movie cannot be deleted if it has active showtimes.
            // Removing a movie is only possible when the system has reached a consistent state with respect to its showtimes.
            CheckRule(new OnlyMoviesWithoutActiveShowtimesCanBeRemovedRule(this.activeShowtimes.AsReadOnly()));

            this.Apply(new MovieRemovedEvent(this.Id));
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
                case MovieAddedEvent @event:
                    this.Applier(@event);
                    break;

                case MovieRemovedEvent:
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
                    throw new EventCannotBeAppliedToAggregateException<Movie, MovieId>(this, domainEvent);
            }
        }

        protected override void EnsureValidState()
        {
            // Not required validation post apply events. This is a deterministic aggregate.
            // All the validations are done on public methods.
        }

        private void Applier(MovieAddedEvent @event)
        {
            this.Id = new MovieId(@event.AggregateId);
            this.Title = new Title(@event.Title);
            this.Runtime = new Runtime(@event.Runtime);
            this.ExhibitionPeriod = new ExhibitionPeriod(
                new StartingPeriod(@event.StartingExhibitionPeriodOnUtc),
                new EndOfPeriod(@event.EndOfExhibitionPeriodOnUtc));
        }
    }
}