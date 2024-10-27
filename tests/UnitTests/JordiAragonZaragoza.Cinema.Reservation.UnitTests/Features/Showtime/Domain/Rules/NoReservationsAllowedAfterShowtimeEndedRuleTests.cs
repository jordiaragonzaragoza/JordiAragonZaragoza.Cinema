namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Showtime.Domain.Rules
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain.Rules;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using Xunit;

    public sealed class NoReservationsAllowedAfterShowtimeEndedRuleTests
    {
        public static IEnumerable<object[]> InvalidArgumentsConstructorNoReservationsAllowedAfterShowtimeEndedRule()
        {
            var showtime = ScheduleShowtimeUtils.Schedule();
            var movie = CreateMovieUtils.Create();
            var createdTimeOnUtc = DateTimeOffset.UtcNow;

            var showtimeValues = new object[] { default!, showtime };
            var movieValues = new object[] { default!, movie };
            var createdTimeOnUtcValues = new object[] { default(DateTimeOffset), createdTimeOnUtc };

            foreach (var showtimeValue in showtimeValues)
            {
                foreach (var movieValue in movieValues)
                {
                    foreach (var createdTimeOnUtcValue in createdTimeOnUtcValues)
                    {
                        if (showtimeValue != null && showtimeValue.Equals(showtime) &&
                            movieValue != null && movieValue.Equals(movie) &&
                            createdTimeOnUtcValue != default && createdTimeOnUtcValue.Equals(createdTimeOnUtc))
                        {
                            continue;
                        }

                        yield return new object[] { showtimeValue!, movieValue!, createdTimeOnUtcValue! };
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
            var showtime = Showtime.Schedule(
                Constants.Showtime.Id,
                Constants.Showtime.MovieId,
                DateTimeOffset.UtcNow,
                Constants.Showtime.AuditoriumId);

            var movie = CreateMovieUtils.Create();
            var createdTimeOnUtc = DateTimeOffset.UtcNow.AddYears(1);

            // Act
            var rule = new NoReservationsAllowedAfterShowtimeEndedRule(showtime, movie, createdTimeOnUtc);

            // Assert
            rule.IsBroken().Should().Be(true);
        }

        [Fact]
        public void IsBroken_WhenHavingCreatedTimeLessThanShowtimeEnded_ShouldBeFalse()
        {
            // Arrange
            var showtime = ScheduleShowtimeUtils.Schedule();
            var movie = CreateMovieUtils.Create();
            var createdTimeOnUtc = DateTimeOffset.UtcNow;

            // Act
            var rule = new NoReservationsAllowedAfterShowtimeEndedRule(showtime, movie, createdTimeOnUtc);

            // Assert
            rule.IsBroken().Should().Be(false);
        }
    }
}