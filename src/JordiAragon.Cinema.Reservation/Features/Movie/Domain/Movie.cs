namespace JordiAragon.Cinema.Reservation.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;
    using JordiAragon.Cinema.Reservation.Movie.Domain.Events;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Entities;
    using JordiAragon.SharedKernel.Domain.Exceptions;

    public sealed class Movie : BaseAggregateRoot<MovieId, Guid>
    {
        private readonly List<ShowtimeId> showtimes = new();

        // Required by EF.
        private Movie()
        {
        }

        // TODO: It belongs to the catalog bounded context.
        public string Title { get; private set; }

        public TimeSpan Runtime { get; private set; }

        public ExhibitionPeriod ExhibitionPeriod { get; private set; }

        public IEnumerable<ShowtimeId> Showtimes => this.showtimes.AsReadOnly();

        public static Movie Add(
            MovieId id,
            string title,
            TimeSpan runtime,
            ExhibitionPeriod exhibitionPeriod)
        {
            var movie = new Movie();

            movie.Apply(new MovieAddedEvent(id, title, runtime, exhibitionPeriod.StartingPeriodOnUtc, exhibitionPeriod.EndOfPeriodOnUtc));

            return movie;
        }

        public void Remove()
            => this.Apply(new MovieRemovedEvent(this.Id));

        public void AddShowtime(ShowtimeId showtimeId)
            => this.Apply(new ShowtimeAddedEvent(this.Id, showtimeId));

        public void RemoveShowtime(ShowtimeId showtimeId)
            => this.Apply(new ShowtimeRemovedEvent(this.Id, showtimeId));

        protected override void When(IDomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case MovieAddedEvent @event:
                    this.Applier(@event);
                    break;

                case MovieRemovedEvent:
                    break;

                case ShowtimeAddedEvent @event:
                    this.showtimes.Add(ShowtimeId.Create(@event.ShowtimeId));
                    break;

                case ShowtimeRemovedEvent @event:
                    this.showtimes.Remove(ShowtimeId.Create(@event.ShowtimeId));
                    break;
            }
        }

        protected override void EnsureValidState()
        {
            try
            {
                Guard.Against.Null(this.Id, nameof(this.Id));
                Guard.Against.NullOrWhiteSpace(this.Title, nameof(this.Title));
                Guard.Against.Default(this.Runtime, nameof(this.Runtime));
                Guard.Against.Null(this.ExhibitionPeriod, nameof(this.ExhibitionPeriod));
            }
            catch (Exception exception)
            {
                throw new InvalidAggregateStateException<Movie, MovieId>(this, exception.Message);
            }
        }

        private void Applier(MovieAddedEvent @event)
        {
            this.Id = MovieId.Create(@event.AggregateId);
            this.Title = @event.Title;
            this.Runtime = @event.Runtime;
            this.ExhibitionPeriod = ExhibitionPeriod.Create(
                StartingPeriod.Create(@event.StartingExhibitionPeriodOnUtc),
                EndOfPeriod.Create(@event.EndOfExhibitionPeriodOnUtc),
                @event.Runtime);
        }
    }
}