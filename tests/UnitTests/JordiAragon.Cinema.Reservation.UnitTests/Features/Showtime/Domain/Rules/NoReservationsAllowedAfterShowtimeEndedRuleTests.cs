namespace JordiAragon.Cinema.Reservation.UnitTests.Showtime.Domain.Rules
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain.Rules;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain;
    using Xunit;

    public class NoReservationsAllowedAfterShowtimeEndedRuleTests
    {
        public static IEnumerable<object[]> InvalidArgumentsConstructorNoReservationsAllowedAfterShowtimeEndedRule()
        {
            var showtime = CreateShowtimeUtils.Create();
            var movie = CreateMovieUtils.Create();
            var createdTimeOnUtc = DateTime.UtcNow;

            var showtimeValues = new object[] { null, showtime };
            var movieValues = new object[] { null, movie };
            var createdTimeOnUtcValues = new object[] { default(DateTimeOffset), createdTimeOnUtc };

            foreach (var showtimeValue in showtimeValues)
            {
                foreach (var movieValue in movieValues)
                {
                    foreach (var createdTimeOnUtcValue in createdTimeOnUtcValues)
                    {
                        if (showtimeValue != null && showtimeValue.Equals(showtime) &&
                            movieValue != null && movieValue.Equals(movie) &&
                            createdTimeOnUtcValue.Equals(createdTimeOnUtc))
                        {
                            continue;
                        }

                        yield return new object[] { showtimeValue, movieValue, createdTimeOnUtcValue };
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsConstructorNoReservationsAllowedAfterShowtimeEndedRule))]
        public void ConstructorNoReservationsAllowedAfterShowtimeEndedRule_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            Showtime showtime,
            Movie movie,
            DateTimeOffset createdTimeOnUtc)
        {
            // Act
            Func<NoReservationsAllowedAfterShowtimeEndedRule> constructor = () => new NoReservationsAllowedAfterShowtimeEndedRule(showtime, movie, createdTimeOnUtc);

            // Assert
            constructor.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void IsBroken_WhenHavingCreatedTimeBiggerThanShowtimeEnded_ShouldBeTrue()
        {
            // Arrange
            var showtime = Showtime.Create(
                Constants.Showtime.Id,
                Constants.Showtime.MovieId,
                DateTime.UtcNow,
                Constants.Showtime.AuditoriumId);

            var movie = CreateMovieUtils.Create();
            var createdTimeOnUtc = DateTime.UtcNow.AddYears(1);

            // Act
            var rule = new NoReservationsAllowedAfterShowtimeEndedRule(showtime, movie, createdTimeOnUtc);

            // Assert
            rule.IsBroken().Should().Be(true);
        }

        [Fact]
        public void IsBroken_WhenHavingCreatedTimeLessThanShowtimeEnded_ShouldBeFalse()
        {
            // Arrange
            var showtime = CreateShowtimeUtils.Create();
            var movie = CreateMovieUtils.Create();
            var createdTimeOnUtc = DateTime.UtcNow;

            // Act
            var rule = new NoReservationsAllowedAfterShowtimeEndedRule(showtime, movie, createdTimeOnUtc);

            // Assert
            rule.IsBroken().Should().Be(false);
        }
    }
}