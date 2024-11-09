namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Events;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using Xunit;

    public sealed class MovieTests
    {
        public static IEnumerable<object[]> InvalidArgumentsAddMovie()
        {
            var id = Constants.Movie.Id;
            var title = Constants.Movie.Title;
            var runtime = Constants.Movie.Runtime;
            var exhibitionPeriod = Constants.Movie.ExhibitionPeriod;

            var idValues = new object[] { default!, id };
            var titleValues = new object[] { default!, title };
            var runtimeValues = new object[] { default!, runtime };
            var exhibitionPeriodValues = new object[] { default!, exhibitionPeriod };

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

                            yield return new object[] { idValue!, titleValue!, runtimeValue!, exhibitionPeriodValue! };
                        }
                    }
                }
            }
        }

        [Fact]
        public void AddMovie_WhenHavingValidArguments_ShouldCreateMovieAndAddMovieCreatedEvent()
        {
            // Arrange
            var id = Constants.Movie.Id;
            var title = Constants.Movie.Title;
            var runtime = Constants.Movie.Runtime;
            var exhibitionPeriod = Constants.Movie.ExhibitionPeriod;

            // Act
            var movie = Movie.Add(id, title, runtime, exhibitionPeriod);

            // Assert
            movie.Should().NotBeNull();
            movie.Id.Should().Be(id);
            movie.Title.Should().Be(title);
            movie.Runtime.Should().Be(runtime);
            movie.ExhibitionPeriod.Should().Be(exhibitionPeriod);

            movie.Events.Should()
                              .ContainSingle(x => x is MovieAddedEvent)
                              .Which.Should().BeOfType<MovieAddedEvent>()
                              .Which.Should().Match<MovieAddedEvent>(e =>
                                                                            e.AggregateId == id &&
                                                                            e.Title == title &&
                                                                            e.Runtime == runtime &&
                                                                            e.StartingExhibitionPeriodOnUtc == exhibitionPeriod.StartingPeriodOnUtc &&
                                                                            e.EndOfExhibitionPeriodOnUtc == exhibitionPeriod.EndOfPeriodOnUtc);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsAddMovie))]
        public void AddMovie_WhenHavingInvalidArguments_ShouldThrowException(
            MovieId id,
            Title title,
            Runtime runtime,
            ExhibitionPeriod exhibitionPeriod)
        {
            // Act
            Func<Movie> movie = () => Movie.Add(id, title, runtime, exhibitionPeriod);

            // Assert
            movie.Should().Throw<Exception>();
        }

        [Fact]
        public void RemoveMovie_WhenHavingValidArguments_ShouldAddMovieRemovedEvent()
        {
            // Arrange.
            var movie = CreateMovieUtils.Create();

            // Act.
            movie.Remove();

            movie.Events.Should()
                              .ContainSingle(x => x is MovieRemovedEvent)
                              .Which.Should().BeOfType<MovieRemovedEvent>()
                              .Which.Should().Match<MovieRemovedEvent>(e => e.AggregateId == movie.Id);
        }

        [Fact]
        public void AddActiveShowtimeToMovie_WhenShowtimeIdIsValid_ShouldAddShowtimeIdAndAddActiveShowtimeAddedEvent()
        {
            // Arrange.
            var movie = CreateMovieUtils.Create();
            var showtimeId = Constants.Showtime.Id;

            // Act.
            movie.AddActiveShowtime(showtimeId);

            // Assert.
            movie.ActiveShowtimes.Should()
                          .Contain(showtimeId)
                          .And
                          .HaveCount(1);

            movie.Events.Should()
                              .ContainSingle(x => x is ActiveShowtimeAddedEvent)
                              .Which.Should().BeOfType<ActiveShowtimeAddedEvent>()
                              .Which.Should().Match<ActiveShowtimeAddedEvent>(e =>
                                                                            e.AggregateId == movie.Id &&
                                                                            e.ShowtimeId == showtimeId);
        }

        [Fact]
        public void RemoveActiveShowtimeToMovie_WhenShowtimeIdIsValid_ShouldAddShowtimeIdAndAddActiveShowtimeRemovedEvent()
        {
            // Arrange.
            var movie = CreateMovieUtils.Create();
            var showtimeId = Constants.Showtime.Id;
            movie.AddActiveShowtime(showtimeId);

            // Act.
            movie.RemoveActiveShowtime(showtimeId);

            // Assert.
            movie.ActiveShowtimes.Should()
                          .NotContain(showtimeId)
                          .And
                          .HaveCount(0);

            movie.Events.Should()
                              .ContainSingle(x => x is ActiveShowtimeRemovedEvent)
                              .Which.Should().BeOfType<ActiveShowtimeRemovedEvent>()
                              .Which.Should().Match<ActiveShowtimeRemovedEvent>(e =>
                                                                            e.AggregateId == movie.Id &&
                                                                            e.ShowtimeId == showtimeId);
        }
    }
}