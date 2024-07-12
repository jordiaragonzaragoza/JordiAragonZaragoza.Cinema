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
        private readonly List<ShowtimeId> activeShowtimes = new();

        // Required by EF.
        private Movie()
        {
        }

        // It belongs to the catalog bounded context but title is inmutable.
        public string Title { get; private set; }

        public Runtime Runtime { get; private set; }

        public ExhibitionPeriod ExhibitionPeriod { get; private set; }

        public IEnumerable<ShowtimeId> ActiveShowtimes => this.activeShowtimes.AsReadOnly();

        public static Movie Add(
            MovieId id,
            string title,
            Runtime runtime,
            ExhibitionPeriod exhibitionPeriod)
        {
            var movie = new Movie();

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
                    this.activeShowtimes.Add(ShowtimeId.Create(@event.ShowtimeId));
                    break;

                case ActiveShowtimeRemovedEvent @event:
                    this.activeShowtimes.Remove(ShowtimeId.Create(@event.ShowtimeId));
                    break;
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
            this.Id = MovieId.Create(@event.AggregateId);
            this.Title = @event.Title;
            this.Runtime = Runtime.Create(@event.Runtime);
            this.ExhibitionPeriod = ExhibitionPeriod.Create(
                StartingPeriod.Create(@event.StartingExhibitionPeriodOnUtc),
                EndOfPeriod.Create(@event.EndOfExhibitionPeriodOnUtc),
                Runtime.Create(@event.Runtime));
        }
    }
}