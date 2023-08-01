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

    public class OnlyContiguousSeatsCanBeReservedRuleTests
    {
        public static IEnumerable<object[]> InvalidArgumentsConstructorOnlyContiguousSeatsCanBeReservedRule()
        {
            yield return new object[] { null };
            yield return new object[] { new List<Seat>() };
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsConstructorOnlyContiguousSeatsCanBeReservedRule))]
        public void ConstructorOnlyContiguousSeatsCanBeReservedRule_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            IEnumerable<Seat> desiredSeats)
        {
            // Act
            Func<OnlyContiguousSeatsCanBeReservedRule> constructor = () => new OnlyContiguousSeatsCanBeReservedRule(desiredSeats);

            // Assert
            constructor.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void IsBroken_WhenSeatsAreContiguous_ShouldBeFalse()
        {
            // Arrange
            var auditorium = CreateAuditoriumUtils.Create();
            var desiredSeats = auditorium.Seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3);

            // Act
            var onlyContiguousSeatsCanBeReservedRule = new OnlyContiguousSeatsCanBeReservedRule(desiredSeats);

            // Assert
            onlyContiguousSeatsCanBeReservedRule.IsBroken().Should().Be(false);
        }

        [Fact]
        public void IsBroken_WhenSeatsAreNotContiguous_ShouldBeTrue()
        {
            // Arrange
            var auditorium = CreateAuditoriumUtils.Create();
            var desiredSeats = auditorium.Seats.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3).ToList();

            desiredSeats.RemoveAt(1);

            // Act
            var onlyContiguousSeatsCanBeReservedRule = new OnlyContiguousSeatsCanBeReservedRule(desiredSeats);

            // Assert
            onlyContiguousSeatsCanBeReservedRule.IsBroken().Should().Be(true);
        }
    }
}