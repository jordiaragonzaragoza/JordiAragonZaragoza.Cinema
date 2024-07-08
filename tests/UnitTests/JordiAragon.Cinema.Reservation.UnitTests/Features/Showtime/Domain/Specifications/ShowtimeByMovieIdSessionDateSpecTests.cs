namespace JordiAragon.Cinema.Reservation.UnitTests.Showtime.Domain.Specifications
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Specifications;
    using JordiAragon.Cinema.Reservation.TestUtilities.Domain;
    using Xunit;

    public sealed class ShowtimeByMovieIdSessionDateSpecTests
    {
        public static IEnumerable<object[]> InvalidArgumentsFindShowtimeByMovieIdSessionDateSpec()
        {
            yield return new object[] { null, default(DateTimeOffset) };
            yield return new object[] { Constants.Movie.Id, default(DateTimeOffset) };
            yield return new object[] { null, DateTimeOffset.UtcNow };
        }

        [Fact]
        public void FindShowtimeByMovieIdSessionDateSpec_WhenHavingValidArguments_ShouldContainTheShowtime()
        {
            // Arrange
            var showtime1 = Showtime.Schedule(
                ShowtimeId.Create(Guid.NewGuid()),
                Constants.Showtime.MovieId,
                Constants.Showtime.SessionDateOnUtc,
                Constants.Showtime.AuditoriumId);

            var showtime2 = Showtime.Schedule(
                ShowtimeId.Create(Guid.NewGuid()),
                Constants.Showtime.MovieId,
                Constants.Showtime.SessionDateOnUtc.AddYears(-1),
                Constants.Showtime.AuditoriumId);

            var showtimes = new List<Showtime>() { showtime1, showtime2 };

            var specification = new ShowtimeByMovieIdSessionDateSpec(
                Constants.Showtime.MovieId,
                Constants.Showtime.SessionDateOnUtc);

            // Act
            var evaluatedList = specification.Evaluate(showtimes);

            // Assert
            evaluatedList.Should()
                         .ContainSingle(c => c == showtime1)
                         .And.NotContain(c => c == showtime2);
        }

        [Fact]
        public void FindShowtimeByMovieIdSessionDateSpec_WhenHavingAnInvalidMovieId_ShouldNotContainTheShowtime()
        {
            // Arrange
            var showtime1 = ScheduleShowtimeUtils.Schedule();

            var showtimes = new List<Showtime>() { showtime1 };

            var specification = new ShowtimeByMovieIdSessionDateSpec(MovieId.Create(Guid.NewGuid()), Constants.Showtime.SessionDateOnUtc);

            // Act
            var evaluatedList = specification.Evaluate(showtimes);

            // Assert
            evaluatedList.Should().BeEmpty();
            evaluatedList.Should()
                         .NotContain(c => c == showtime1);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsFindShowtimeByMovieIdSessionDateSpec))]
        public void FindShowtimeByMovieIdSessionDateSpec_WhenHavingInvalidArguments_ShouldThrowArgumentNullException(
            MovieId movieId,
            DateTimeOffset sessionDateOnUtc)
        {
            // Act
            Func<ShowtimeByMovieIdSessionDateSpec> showtime = () => new ShowtimeByMovieIdSessionDateSpec(movieId, sessionDateOnUtc);

            // Assert
            showtime.Should().Throw<ArgumentException>();
        }
    }
}