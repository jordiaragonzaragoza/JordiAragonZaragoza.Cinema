﻿namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Specifications
{
    using System;
    using System.Linq;
    using Ardalis.GuardClauses;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;

    public sealed class ShowtimeByMovieIdSessionDateSpec : SingleResultSpecification<Showtime>
    {
        public ShowtimeByMovieIdSessionDateSpec(MovieId movieId, DateTimeOffset sessionDateOnUtc)
        {
            ArgumentNullException.ThrowIfNull(movieId);
            Guard.Against.Default(sessionDateOnUtc);

            this.Query
                .Where(showtime => showtime.MovieId == movieId
                                && showtime.SessionDateOnUtc == sessionDateOnUtc);
        }
    }
}