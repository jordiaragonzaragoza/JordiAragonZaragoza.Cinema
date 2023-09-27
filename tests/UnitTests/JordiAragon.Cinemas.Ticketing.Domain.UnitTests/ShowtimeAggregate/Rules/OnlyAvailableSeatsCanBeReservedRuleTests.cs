namespace JordiAragon.Cinemas.Ticketing.Domain.UnitTests.ShowtimeAggregate.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Domain.AuditoriumAggregate;
    using JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate.Rules;
    using JordiAragon.Cinemas.Ticketing.Domain.UnitTests.Features.Auditorium.TestUtils;
    using Xunit;

    public class OnlyAvailableSeatsCanBeReservedRuleTests
    {
        public static IEnumerable<object[]> InvalidArgumentsConstructorOnlyAvailableSeatsCanBeReservedRule()
        {
            var auditorium = CreateAuditoriumUtils.Create();
            var availableSeats = auditorium.Seats;
            var desiredSeats = auditorium.Seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3);

            var desiredSeatsValues = new object[] { null, new List<Seat>(), desiredSeats };
            var availableSeatsValues = new object[] { null, new List<Seat>(), availableSeats };

            foreach (var desiredSeatValue in desiredSeatsValues)
            {
                foreach (var availableSeatValue in availableSeatsValues)
                {
                    if (desiredSeatValue != null && desiredSeatValue.Equals(desiredSeats) &&
                        availableSeatValue != null && availableSeatValue.Equals(availableSeats))
                    {
                        continue;
                    }

                    yield return new object[] { desiredSeatValue, availableSeatValue };
                }
            }
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsConstructorOnlyAvailableSeatsCanBeReservedRule))]
        public void ConstructorOnlyAvailableSeatsCanBeReservedRule_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            IEnumerable<Seat> desiredSeats,
            IEnumerable<Seat> availableSeats)
        {
            // Act
            Func<OnlyAvailableSeatsCanBeReservedRule> constructor = () => new OnlyAvailableSeatsCanBeReservedRule(desiredSeats, availableSeats);

            // Assert
            constructor.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void IsBroken_WhenHavingDesiredReservedSeats_ShouldBeTrue()
        {
            // Arrange
            var auditorium = CreateAuditoriumUtils.Create();
            var availableSeats = auditorium.Seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3);
            var desiredSeats = auditorium.Seats;

            // Act
            var rule = new OnlyAvailableSeatsCanBeReservedRule(desiredSeats, availableSeats);

            // Assert
            rule.IsBroken().Should().Be(true);
        }

        [Fact]
        public void IsBroken_WhenHavingDesiredReservedSeats_ShouldBeFalse()
        {
            // Arrange
            var auditorium = CreateAuditoriumUtils.Create();
            var availableSeats = auditorium.Seats;
            var desiredSeats = auditorium.Seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3);

            // Act
            var rule = new OnlyAvailableSeatsCanBeReservedRule(desiredSeats, availableSeats);

            // Assert
            rule.IsBroken().Should().Be(false);
        }
    }
}