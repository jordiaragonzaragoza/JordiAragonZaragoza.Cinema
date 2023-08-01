namespace JordiAragon.Cinema.Domain.UnitTests.ShowtimeAggregate.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate.Rules;
    using JordiAragon.Cinema.Domain.UnitTests.Features.Auditorium.TestUtils;
    using Xunit;

    public class OnlyAvailableSeatsCanBeReservedRuleTests
    {
        public static IEnumerable<object[]> InvalidArgumentsConstructorOnlyAvailableSeatsCanBeReservedRule()
        {
            var auditorium = CreateAuditoriumUtils.Create();
            var availableSeats = auditorium.Seats;
            var desiredSeats = auditorium.Seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3);

            yield return new object[] { null, null };
            yield return new object[] { desiredSeats, null };
            yield return new object[] { null, availableSeats };
            yield return new object[] { desiredSeats, new List<Seat>() };
            yield return new object[] { new List<Seat>(), availableSeats };
            yield return new object[] { null, new List<Seat>() };
            yield return new object[] { new List<Seat>(), null };
            yield return new object[] { new List<Seat>(), new List<Seat>() };
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