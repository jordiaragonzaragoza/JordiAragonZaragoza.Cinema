namespace JordiAragon.Cinema.Reservation.UnitTests.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain.Events;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain;
    using JordiAragon.SharedKernel.Domain.Exceptions;
    using Xunit;

    public class MovieTests
    {
        public static IEnumerable<object[]> InvalidArgumentsCreateMovie()
        {
            var title = Constants.Movie.Title;
            var runtime = Constants.Movie.Runtime;

            var titleValues = new object[] { null, string.Empty, " ", title };
            var runtimeValues = new object[] { default(TimeSpan), runtime };

            foreach (var titleValue in titleValues)
            {
                foreach (var imdbIdValue in runtimeValues)
                {
                    if (titleValue != null && titleValue.Equals(title) &&
                                imdbIdValue != null && imdbIdValue.Equals(runtime))
                    {
                        continue;
                    }

                    yield return new object[] { titleValue, imdbIdValue };
                }
            }
        }

        [Fact]
        public void CreateMovie_WhenHavingValidArguments_ShouldCreateMovieAndAddMovieCreatedEvent()
        {
            // Arrange
            var id = Constants.Movie.Id;
            var title = Constants.Movie.Title;
            var runtime = Constants.Movie.Runtime;

            // Act
            var movie = Movie.Create(id, title, runtime);

            // Assert
            movie.Should().NotBeNull();
            movie.Id.Should().Be(id);

            movie.Events.Should()
                              .ContainSingle(x => x is MovieCreatedEvent)
                              .Which.Should().BeOfType<MovieCreatedEvent>()
                              .Which.Should().Match<MovieCreatedEvent>(e =>
                                                                            e.MovieId == id &&
                                                                            e.Title == title &&
                                                                            e.Runtime == runtime);
        }

        [Fact]
        public void CreateMovie_WhenHavingInCorrectMovieIdArgument_ShouldThrowArgumentNullException()
        {
            // Arrange
            MovieId id = null;
            var title = Constants.Movie.Title;
            var runtime = Constants.Movie.Runtime;

            // Act
            Func<Movie> movie = () => Movie.Create(id, title, runtime);

            // Assert
            movie.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateMovie))]
        public void CreateMovie_WhenHavingInvalidArguments_ShouldThrowInvalidAggregateStateException(
            string title,
            TimeSpan runtime)
        {
            // Arrange
            var id = Constants.Movie.Id;

            // Act
            Func<Movie> movie = () => Movie.Create(id, title, runtime);

            // Assert
            movie.Should().Throw<InvalidAggregateStateException<Movie, MovieId>>();
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
                              .ContainSingle(x => x is ShowtimeAddedEvent)
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
                              .ContainSingle(x => x is ShowtimeRemovedEvent)
                              .Which.Should().BeOfType<ShowtimeRemovedEvent>()
                              .Which.Should().Match<ShowtimeRemovedEvent>(e =>
                                                                            e.MovieId == movie.Id &&
                                                                            e.ShowtimeId == showtimeId);
        }
    }
}