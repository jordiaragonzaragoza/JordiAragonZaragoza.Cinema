namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Movie.Domain.Rules
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using Xunit;

    public sealed class EndOfPeriodShouldBeBiggerThanStartingPeriodRuleTests
    {
        public static IEnumerable<object[]> InvalidArgumentsConstructorConstructorExhibitionPeriodMustExceedOrEqualRuntimeRule()
        {
            var startingPeriod = Constants.Movie.StartingPeriod;
            var endOfPeriod = Constants.Movie.EndOfPeriod;

            var startingPeriodValues = new object[] { default!, startingPeriod };
            var endOfPeriodValues = new object[] { default!, endOfPeriod };

            foreach (var startingPeriodValue in startingPeriodValues)
            {
                foreach (var endOfPeriodValue in endOfPeriodValues)
                {
                    if (startingPeriodValue != null && startingPeriodValue.Equals(startingPeriod) &&
                        endOfPeriodValue != null && endOfPeriodValue.Equals(endOfPeriod))
                    {
                        continue;
                    }

                    yield return new object[] { startingPeriodValue!, endOfPeriodValue! };
                }
            }
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsConstructorConstructorExhibitionPeriodMustExceedOrEqualRuntimeRule))]
        public void ConstructorEndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRule_WhenHavingInvalidArguments_ShouldThrowArgumentNullException(
            StartingPeriod startingPeriod,
            EndOfPeriod endOfPeriod)
        {
            // Act
            Func<EndOfPeriodShouldBeBiggerThanStartingPeriodRule> sut = () => new EndOfPeriodShouldBeBiggerThanStartingPeriodRule(endOfPeriod, startingPeriod);

            // Assert
            sut.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ConstructorEndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRule_WhenHavingAValidExhibitionPeriod_ShouldReturnEndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRule()
        {
            // Arrange
            var endOfPeriod = Constants.Movie.EndOfPeriod;
            var startingPeriod = Constants.Movie.StartingPeriod;

            // Act
            var rule = new EndOfPeriodShouldBeBiggerThanStartingPeriodRule(endOfPeriod, startingPeriod);

            // Assert
            rule.Should().NotBeNull();
        }

        [Fact]
        public void IsBroken_WhenHavingBiggerEndOfPeriodThanStartingPeriod_ShouldBeFalse()
        {
            // Arrange
            var startingPeriod = StartingPeriod.Create(new DateTimeOffset(DateTimeOffset.UtcNow.AddYears(1).Year, 1, 1, 1, 1, 1, 1, TimeSpan.Zero));
            var endOfPeriod = EndOfPeriod.Create(DateTimeOffset.UtcNow.AddYears(2));

            // Act
            var rule = new EndOfPeriodShouldBeBiggerThanStartingPeriodRule(endOfPeriod, startingPeriod);

            // Assert
            rule.IsBroken().Should().Be(false);
        }
    }
}