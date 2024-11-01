namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Movie.Domain
{
    using System;
    using System.Globalization;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using Xunit;

    public sealed class StartingPeriodTests
    {
        [Fact]
        public void CreateStartingPeriod_WhenHavingADefaultDateTimeOffset_ShouldThrowArgumentException()
        {
            // Arrange
            var value = default(DateTimeOffset);

            // Act
            Func<StartingPeriod> startingPeriod = () => StartingPeriod.Create(value);

            // Assert
            startingPeriod.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateStartingPeriod_WhenHavingAValidDateTimeOffset_ShouldReturnStartingPeriod()
        {
            // Arrange
            var value = DateTimeOffset.UtcNow;

            // Act
            var startingPeriod = StartingPeriod.Create(value);

            // Assert
            startingPeriod.Should().NotBeNull();
        }

        [Fact]
        public void ImplicitConversion_WhenHavingAStartingPeriod_ShouldReturnDateTimeOffset()
        {
            // Arrange
            var startingPeriodValue = DateTimeOffset.UtcNow;
            var startingPeriod = StartingPeriod.Create(startingPeriodValue);

            // Act
            DateTimeOffset result = startingPeriod;

            // Assert
            result.Should().Be(startingPeriodValue);
        }

        [Fact]
        public void ExplicitConversion_WhenHavingADateTimeOffset_ShouldReturnStartingPeriod()
        {
            // Arrange
            var dateTimeOffsetValue = DateTimeOffset.UtcNow;

            // Act
            StartingPeriod startingPeriod = (StartingPeriod)dateTimeOffsetValue;

            // Assert
            startingPeriod.Should().NotBeNull();
            startingPeriod.Value.Should().Be(dateTimeOffsetValue);
        }

        [Fact]
        public void ToString_ShouldReturnStringRepresentationOfStartingPeriod()
        {
            // Arrange
            var value = DateTimeOffset.UtcNow;
            var startingPeriod = StartingPeriod.Create(value);

            // Act
            var result = startingPeriod.ToString();

            // Assert
            result.Should().Be(value.ToString(CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Equality_Checks_ShouldWorkAsExpected()
        {
            // Arrange
            var value1 = DateTimeOffset.UtcNow;
            var value2 = value1.AddMinutes(1);

            var startingPeriod1 = StartingPeriod.Create(value1);
            var startingPeriod2 = StartingPeriod.Create(value1);
            var startingPeriod3 = StartingPeriod.Create(value2);

            // Act & Assert
            startingPeriod1.Should().Be(startingPeriod2);
            startingPeriod1.Should().NotBe(startingPeriod3);
        }
    }
}