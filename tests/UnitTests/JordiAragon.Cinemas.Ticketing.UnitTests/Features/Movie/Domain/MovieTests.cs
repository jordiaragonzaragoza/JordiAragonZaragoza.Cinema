namespace JordiAragon.Cinemas.Ticketing.UnitTests.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Movie.Domain;
    using JordiAragon.Cinemas.Ticketing.Movie.Domain.Events;
    using JordiAragon.Cinemas.Ticketing.UnitTests.TestUtils.Domain;
    using JordiAragon.SharedKernel.Domain.Exceptions;
    using Xunit;

    public class MovieTests
    {
        public static IEnumerable<object[]> InvalidArgumentsCreateMovie()
        {
            var title = Constants.Movie.Title;
            var imdbId = Constants.Movie.ImdbId;
            var releaseDateOnUtc = Constants.Movie.ReleaseDateOnUtc;
            var stars = Constants.Movie.Stars;

            var titleValues = new object[] { null, string.Empty, " ", title };
            var imdbIdValues = new object[] { null, string.Empty, " ", imdbId };
            var relaseDateOnUtcValues = new object[] { default(DateTime), releaseDateOnUtc };
            var starsValues = new object[] { null, string.Empty, " ", stars };

            foreach (var titleValue in titleValues)
            {
                foreach (var imdbIdValue in imdbIdValues)
                {
                    foreach (var releaseDateOnUtcValue in relaseDateOnUtcValues)
                    {
                        foreach (var starsValue in starsValues)
                        {
                            if (titleValue != null && titleValue.Equals(title) &&
                                imdbIdValue != null && imdbIdValue.Equals(imdbId) &&
                                releaseDateOnUtcValue.Equals(releaseDateOnUtc) &&
                                starsValue != null && starsValue.Equals(stars))
                            {
                                continue;
                            }

                            yield return new object[] { titleValue, imdbIdValue, releaseDateOnUtcValue, starsValue };
                        }
                    }
                }
            }
        }

        [Fact]
        public void CreateMovie_WhenHavingValidArguments_ShouldCreateMovieAndAddMovieCreatedEvent()
        {
            // Arrange
            var id = Constants.Movie.Id;
            var title = Constants.Movie.Title;
            var imdbId = Constants.Movie.ImdbId;
            var releaseDateOnUtc = Constants.Movie.ReleaseDateOnUtc;
            var stars = Constants.Movie.Stars;

            // Act
            var movie = Movie.Create(id, title, imdbId, releaseDateOnUtc, stars);

            // Assert
            movie.Should().NotBeNull();
            movie.Id.Should().Be(id);

            movie.Events.Should()
                              .ContainSingle(x => x.GetType() == typeof(MovieCreatedEvent))
                              .Which.Should().BeOfType<MovieCreatedEvent>()
                              .Which.Should().Match<MovieCreatedEvent>(e =>
                                                                            e.MovieId == id &&
                                                                            e.Title == title &&
                                                                            e.ImdbId == imdbId &&
                                                                            e.ReleaseDateOnUtc == releaseDateOnUtc &&
                                                                            e.Stars == stars);
        }

        [Fact]
        public void CreateMovie_WhenHavingInCorrectMovieIdArgument_ShouldThrowArgumentNullException()
        {
            // Arrange
            MovieId id = null;
            var title = Constants.Movie.Title;
            var imdbId = Constants.Movie.ImdbId;
            var releaseDateOnUtc = Constants.Movie.ReleaseDateOnUtc;
            var stars = Constants.Movie.Stars;

            // Act
            Func<Movie> movie = () => Movie.Create(id, title, imdbId, releaseDateOnUtc, stars);

            // Assert
            movie.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateMovie))]
        public void CreateMovie_WhenHavingInvalidArguments_ShouldThrowInvalidAggregateStateException(
            string title,
            string imdbId,
            DateTime relaseDateOnUtc,
            string stars)
        {
            // Arrange
            var id = Constants.Movie.Id;

            // Act
            Func<Movie> movie = () => Movie.Create(id, title, imdbId, relaseDateOnUtc, stars);

            // Assert
            movie.Should().Throw<InvalidAggregateStateException>();
        }

        [Fact]
        public void AddShowtimeToMovie_WhenShowtimeIdIsValid_ShouldAddShowtimeIdAndAddShowtimeAddedEvent()
        {
            // Arrange.
            var movie = CreateMovieUtils.Create();
            var showtimeId = Constants.Showtime.Id;

            // Act.
            movie.AddShowtime(showtimeId);

            // Assert.
            movie.Showtimes.Should()
                          .Contain(showtimeId)
                          .And
                          .HaveCount(1);

            movie.Events.Should()
                              .ContainSingle(x => x.GetType() == typeof(ShowtimeAddedEvent))
                              .Which.Should().BeOfType<ShowtimeAddedEvent>()
                              .Which.Should().Match<ShowtimeAddedEvent>(e =>
                                                                            e.MovieId == movie.Id &&
                                                                            e.ShowtimeId == showtimeId);
        }

        [Fact]
        public void RemoveShowtimeToMovie_WhenShowtimeIdIsValid_ShouldAddShowtimeIdAndAddShowtimeRemovedEvent()
        {
            // Arrange.
            var movie = CreateMovieUtils.Create();
            var showtimeId = Constants.Showtime.Id;
            movie.AddShowtime(showtimeId);

            // Act.
            movie.RemoveShowtime(showtimeId);

            // Assert.
            movie.Showtimes.Should()
                          .NotContain(showtimeId)
                          .And
                          .HaveCount(0);

            movie.Events.Should()
                              .ContainSingle(x => x.GetType() == typeof(ShowtimeRemovedEvent))
                              .Which.Should().BeOfType<ShowtimeRemovedEvent>()
                              .Which.Should().Match<ShowtimeRemovedEvent>(e =>
                                                                            e.MovieId == movie.Id &&
                                                                            e.ShowtimeId == showtimeId);
        }
    }
}