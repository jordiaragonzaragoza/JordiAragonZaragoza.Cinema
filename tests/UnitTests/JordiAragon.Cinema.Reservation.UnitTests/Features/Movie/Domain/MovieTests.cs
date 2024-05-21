namespace JordiAragon.Cinema.Reservation.UnitTests.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain.Events;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain;
    using Xunit;

    public sealed class MovieTests
    {
        public static IEnumerable<object[]> InvalidArgumentsCreateMovie()
        {
            var id = Constants.Movie.Id;
            var title = Constants.Movie.Title;
            var runtime = Constants.Movie.Runtime;
            var exhibitionPeriod = Constants.Movie.ExhibitionPeriod;

            var idValues = new object[] { null, id };
            var titleValues = new object[] { null, string.Empty, " ", title };
            var runtimeValues = new object[] { default(TimeSpan), runtime };
            var exhibitionPeriodValues = new object[] { null, exhibitionPeriod };

            foreach (var idValue in idValues)
            {
                foreach (var titleValue in titleValues)
                {
                    foreach (var runtimeValue in runtimeValues)
                    {
                        foreach (var exhibitionPeriodValue in exhibitionPeriodValues)
                        {
                            if (idValue != null && idValue.Equals(id) &&
                                    titleValue != null && titleValue.Equals(title) &&
                                    runtimeValue != null && runtimeValue.Equals(runtime) &&
                                    exhibitionPeriodValue != null && exhibitionPeriodValue.Equals(exhibitionPeriod))
                            {
                                continue;
                            }

                            yield return new object[] { idValue, titleValue, runtimeValue, exhibitionPeriodValue };
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
            var runtime = Constants.Movie.Runtime;
            var exhibitionPeriod = Constants.Movie.ExhibitionPeriod;

            // Act
            var movie = Movie.Create(id, title, runtime, exhibitionPeriod);

            // Assert
            movie.Should().NotBeNull();
            movie.Id.Should().Be(id);
            movie.Title.Should().Be(title);
            movie.Runtime.Should().Be(runtime);
            movie.ExhibitionPeriod.Should().Be(exhibitionPeriod);

            movie.Events.Should()
                              .ContainSingle(x => x is MovieCreatedEvent)
                              .Which.Should().BeOfType<MovieCreatedEvent>()
                              .Which.Should().Match<MovieCreatedEvent>(e =>
                                                                            e.AggregateId == id &&
                                                                            e.Title == title &&
                                                                            e.Runtime == runtime &&
                                                                            e.StartingExhibitionPeriodOnUtc == exhibitionPeriod.StartingPeriodOnUtc &&
                                                                            e.EndOfExhibitionPeriodOnUtc == exhibitionPeriod.EndOfPeriodOnUtc);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateMovie))]
        public void CreateMovie_WhenHavingInvalidArguments_ShouldThrowException(
            MovieId id,
            string title,
            TimeSpan runtime,
            ExhibitionPeriod exhibitionPeriod)
        {
            // Act
            Func<Movie> movie = () => Movie.Create(id, title, runtime, exhibitionPeriod);

            // Assert
            movie.Should().Throw<Exception>();
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
                                                                            e.AggregateId == movie.Id &&
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
                                                                            e.AggregateId == movie.Id &&
                                                                            e.ShowtimeId == showtimeId);
        }
    }
}