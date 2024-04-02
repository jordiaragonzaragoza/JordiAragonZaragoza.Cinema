namespace JordiAragon.Cinema.Reservation.UnitTests.Movie.Domain.Rules
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Movie.Domain.Rules;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain;
    using Xunit;

    public sealed class EndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRuleTests
    {
        [Fact]
        public void ConstructorEndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRule_WhenHavingANullExhibitionPeriod_ShouldThrowArgumentNullException()
        {
            // Arrange
            ExhibitionPeriod exhibitionPeriod = null;

            // Act
            Func<EndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRule> sut = () => new EndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRule(exhibitionPeriod);

            // Assert
            sut.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ConstructorEndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRule_WhenHavingAValidExhibitionPeriod_ShouldReturnEndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRule()
        {
            // Arrange
            var exhibitionPeriod = Constants.Movie.ExhibitionPeriod;

            // Act
            var rule = new EndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRule(exhibitionPeriod);

            // Assert
            rule.Should().NotBeNull();
        }

        [Fact]
        public void IsBroken_WhenHavingBiggerEndOfPeriodThanStartingPeriod_ShouldBeFalse()
        {
            // Arrange
            var startingPeriod = StartingPeriod.Create(new DateTimeOffset(DateTimeOffset.UtcNow.AddYears(1).Year, 1, 1, 1, 1, 1, 1, TimeSpan.Zero));
            var endOfPeriod = EndOfPeriod.Create(DateTimeOffset.UtcNow.AddYears(2));
            var runtime = Constants.Movie.Runtime;

            var exhibitionPeriod = ExhibitionPeriod.Create(
                    startingPeriod,
                    endOfPeriod,
                    runtime);

            // Act
            var rule = new EndOfPeriodShouldBeBiggerThanStartingPeriodPeriodRule(exhibitionPeriod);

            // Assert
            rule.IsBroken().Should().Be(false);
        }
    }
}