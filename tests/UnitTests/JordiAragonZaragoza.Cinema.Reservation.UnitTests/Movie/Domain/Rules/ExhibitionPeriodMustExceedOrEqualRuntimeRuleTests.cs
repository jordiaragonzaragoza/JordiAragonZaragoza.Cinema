namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Movie.Domain.Rules
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using Xunit;

    public sealed class ExhibitionPeriodMustExceedOrEqualRuntimeRuleTests
    {
        public static IEnumerable<object[]> InvalidArgumentsConstructorConstructorExhibitionPeriodMustExceedOrEqualRuntimeRule()
        {
            var startingPeriod = Constants.Movie.StartingPeriod;
            var endOfPeriod = Constants.Movie.EndOfPeriod;
            var runtime = Constants.Movie.Runtime;

            var startingPeriodValues = new object[] { default!, startingPeriod };
            var endOfPeriodValues = new object[] { default!, endOfPeriod };
            var runtimeValues = new object[] { default!, runtime };

            foreach (var startingPeriodValue in startingPeriodValues)
            {
                foreach (var endOfPeriodValue in endOfPeriodValues)
                {
                    foreach (var runtimeValue in runtimeValues)
                    {
                        if (startingPeriodValue != null && startingPeriodValue.Equals(startingPeriod) &&
                            endOfPeriodValue != null && endOfPeriodValue.Equals(endOfPeriod) &&
                            runtimeValue != null && runtimeValue.Equals(runtime))
                        {
                            continue;
                        }

                        yield return new object[] { startingPeriodValue!, endOfPeriodValue!, runtimeValue! };
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsConstructorConstructorExhibitionPeriodMustExceedOrEqualRuntimeRule))]
        public void ConstructorExhibitionPeriodMustExceedOrEqualRuntimeRule_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            StartingPeriod startingPeriod,
            EndOfPeriod endOfPeriod,
            Runtime runtime)
        {
            // Act
            Func<ExhibitionPeriodMustExceedOrEqualRuntimeRule> constructor = () => new ExhibitionPeriodMustExceedOrEqualRuntimeRule(startingPeriod, endOfPeriod, runtime);

            // Assert
            constructor.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ConstructorExhibitionPeriodMustExceedOrEqualRuntimeRule_WhenHavingAValidArguments_ShouldReturnExhibitionPeriodMustExceedOrEqualRuntimeRule()
        {
            // Arrange
            var startingPeriod = Constants.Movie.StartingPeriod;
            var endOfPeriod = Constants.Movie.EndOfPeriod;
            var runtime = Constants.Movie.Runtime;

            // Act
            var rule = new ExhibitionPeriodMustExceedOrEqualRuntimeRule(startingPeriod, endOfPeriod, runtime);

            // Assert
            rule.Should().NotBeNull();
        }

        [Fact]
        public void IsBroken_WhenHavingBiggerEndOfPeriodThanStartingPeriod_ShouldBeFalse()
        {
            // Arrange
            var startingPeriod = StartingPeriod.Create(new DateTimeOffset(DateTimeOffset.UtcNow.AddYears(1).Year, 1, 1, 1, 1, 1, 1, TimeSpan.Zero));
            var endOfPeriod = EndOfPeriod.Create(DateTimeOffset.UtcNow.AddYears(2));
            var runtime = Runtime.Create(TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28));

            // Act
            var rule = new ExhibitionPeriodMustExceedOrEqualRuntimeRule(startingPeriod, endOfPeriod, runtime);

            // Assert
            rule.IsBroken().Should().Be(false);
        }
    }
}