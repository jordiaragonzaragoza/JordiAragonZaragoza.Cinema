namespace JordiAragonZaragoza.Cinema.Reservation.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Events;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Domain.Entities;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;

    public sealed class Movie : BaseAggregateRoot<MovieId, Guid>
    {
        private readonly List<ShowtimeId> activeShowtimes = new();

        // Required by EF.
        private Movie()
        {
        }

        // It belongs to the catalog bounded context but title is inmutable.
        public string Title { get; private set; } = default!;

        public Runtime Runtime { get; private set; } = default!;

        public ExhibitionPeriod ExhibitionPeriod { get; private set; } = default!;

        public IEnumerable<ShowtimeId> ActiveShowtimes => this.activeShowtimes.AsReadOnly();

        public static Movie Add(
            MovieId id,
            string title,
            Runtime runtime,
            ExhibitionPeriod exhibitionPeriod)
        {
            var movie = new Movie();

            Guard.Against.Null(exhibitionPeriod, nameof(exhibitionPeriod));

            movie.Apply(new MovieAddedEvent(id, title, runtime, exhibitionPeriod.StartingPeriodOnUtc, exhibitionPeriod.EndOfPeriodOnUtc));

            return movie;
        }

        public void Remove()
            => this.Apply(new MovieRemovedEvent(this.Id));

        public void AddActiveShowtime(ShowtimeId showtimeId)
            => this.Apply(new ActiveShowtimeAddedEvent(this.Id, showtimeId));

        public void RemoveActiveShowtime(ShowtimeId showtimeId)
            => this.Apply(new ActiveShowtimeRemovedEvent(this.Id, showtimeId));

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case MovieAddedEvent @event:
                    this.Applier(@event);
                    break;

                case MovieRemovedEvent:
                    break;

                case ActiveShowtimeAddedEvent @event:
                    this.activeShowtimes.Add(new ShowtimeId(@event.ShowtimeId));
                    break;

                case ActiveShowtimeRemovedEvent @event:
                    this.activeShowtimes.Remove(new ShowtimeId(@event.ShowtimeId));
                    break;

                default:
                    throw new EventCannotBeAppliedToAggregateException<Movie, MovieId>(this, domainEvent);
            }
        }

        protected override void EnsureValidState()
        {
            try
            {
                Guard.Against.Null(this.Id, nameof(this.Id));
                Guard.Against.NullOrWhiteSpace(this.Title, nameof(this.Title));
                Guard.Against.Null(this.Runtime, nameof(this.Runtime));
                Guard.Against.Null(this.ExhibitionPeriod, nameof(this.ExhibitionPeriod));
            }
            catch (Exception exception)
            {
                throw new InvalidAggregateStateException<Movie, MovieId>(this, exception.Message);
            }
        }

        private void Applier(MovieAddedEvent @event)
        {
            this.Id = new MovieId(@event.AggregateId);
            this.Title = @event.Title;
            this.Runtime = new Runtime(@event.Runtime);
            this.ExhibitionPeriod = new ExhibitionPeriod(
                new StartingPeriod(@event.StartingExhibitionPeriodOnUtc),
                new EndOfPeriod(@event.EndOfExhibitionPeriodOnUtc));
        }
    }
}