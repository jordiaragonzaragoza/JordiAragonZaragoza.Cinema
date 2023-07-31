namespace JordiAragon.Cinema.Domain.UnitTests.MovieAggregate
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate.Events;
    using JordiAragon.Cinema.Domain.UnitTests.Features.Movie.TestUtils;
    using JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants;
    using JordiAragon.SharedKernel.Domain.Exceptions;
    using Xunit;

    public class MovieTests
    {
        public static IEnumerable<object[]> InvalidArgumentsCreateMovie()
        {
            yield return new object[] { null, null, default(DateTime), string.Empty };
            yield return new object[] { "Inception", null, default(DateTime), string.Empty };
            yield return new object[] { null, "tt1375666", default(DateTime), string.Empty };
            yield return new object[] { null, null, new DateTime(2010, 01, 14), string.Empty };
            yield return new object[] { null, null, default(DateTime), "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page, Ken Watanabe" };
            yield return new object[] { "Inception", "tt1375666", default(DateTime), string.Empty };
            yield return new object[] { "Inception", null, new DateTime(2010, 01, 14), string.Empty };
            yield return new object[] { "Inception", null, default(DateTime), "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page, Ken Watanabe" };
            yield return new object[] { null, "tt1375666", new DateTime(2010, 01, 14), string.Empty };
            yield return new object[] { null, "tt1375666", default(DateTime), "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page, Ken Watanabe" };
            yield return new object[] { null, null, new DateTime(2010, 01, 14), "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page, Ken Watanabe" };
            yield return new object[] { "Inception", "tt1375666", new DateTime(2010, 01, 14), string.Empty };
            yield return new object[] { "Inception", "tt1375666", default(DateTime), "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page, Ken Watanabe" };
            yield return new object[] { "Inception", null, new DateTime(2010, 01, 14), "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page, Ken Watanabe" };
            yield return new object[] { null, "tt1375666", new DateTime(2010, 01, 14), "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page, Ken Watanabe" };
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