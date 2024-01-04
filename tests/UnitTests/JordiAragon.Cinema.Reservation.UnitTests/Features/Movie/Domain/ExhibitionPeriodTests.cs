namespace JordiAragon.Cinema.Reservation.UnitTests.Movie.Domain
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain;
    using Xunit;

    public class ExhibitionPeriodTests
    {
        public static IEnumerable<object[]> InvalidArgumentsCreateExhibitionPeriod()
        {
            var startingPeriod = Constants.Movie.StartingPeriod;
            var endOfPeriod = Constants.Movie.EndOfPeriod;
            var runtime = Constants.Movie.Runtime;

            var startingPeriodValues = new object[] { null, default(StartingPeriod), startingPeriod };
            var endOfPeriodValues = new object[] { null, default(EndOfPeriod), startingPeriod, endOfPeriod };
            var runtimeValues = new object[] { default(TimeSpan), runtime };

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

                        yield return new object[] { startingPeriodValue, endOfPeriodValue, runtimeValue };
                    }
                }
            }
        }

        /*
        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateExhibitionPeriod))]
        public void CreateExhibitionPeriod_WhenHavingInvalidArguments_ShouldThrowException(
            StartingPeriod startingPeriod,
            EndOfPeriod endOfPeriod,
            TimeSpan runtime)
        {
            // Act
            Func<ExhibitionPeriod> exhibitionPeriod = () => ExhibitionPeriod.Create(startingPeriod, endOfPeriod, runtime);

            // Assert
            exhibitionPeriod.Should().Throw<Exception>();
        }*/

        [Fact]
        public void CreateExhibitionPeriod_WhenHavingValidArguments_ShouldCreateExhibitionPeriod()
        {
            // Arrange
            var startingPeriod = Constants.Movie.StartingPeriod;
            var endOfPeriod = Constants.Movie.EndOfPeriod;
            var runtime = Constants.Movie.Runtime;

            // Act
            var exhibitionPeriod = ExhibitionPeriod.Create(startingPeriod, endOfPeriod, runtime);

            // Assert
            exhibitionPeriod.Should().NotBeNull();
            exhibitionPeriod.StartingPeriodOnUtc.Should().Be(startingPeriod);
            exhibitionPeriod.EndOfPeriodOnUtc.Should().Be(endOfPeriod);
        }

        [Fact]
        public void ToString_ShouldReturnStringRepresentationOfEndOfPeriod()
        {
            // Arrange
            var exhibitionPeriod = Constants.Movie.ExhibitionPeriod;

            // Act
            var result = exhibitionPeriod.ToString();

            // Assert
            result.Should().Be($"StartingPeriodOnUtc: {exhibitionPeriod.StartingPeriodOnUtc} EndOfPeriodOnUtc: {exhibitionPeriod.EndOfPeriodOnUtc}");
        }

        [Fact]
        public void Equality_Checks_ShouldWorkAsExpected()
        {
            // Arrange
            var startingPeriod = Constants.Movie.StartingPeriod;
            var endOfPeriod = Constants.Movie.EndOfPeriod;
            var runtime = Constants.Movie.Runtime;

            DateTimeOffset newStartingPeriod = Constants.Movie.StartingPeriod;
            var startingPeriod2 = (StartingPeriod)newStartingPeriod.AddDays(1);

            var exhibitionPeriod1 = ExhibitionPeriod.Create(startingPeriod, endOfPeriod, runtime);
            var exhibitionPeriod2 = ExhibitionPeriod.Create(startingPeriod, endOfPeriod, runtime);
            var exhibitionPeriod3 = ExhibitionPeriod.Create(startingPeriod2, endOfPeriod, runtime);

            // Act & Assert
            exhibitionPeriod1.Should().Be(exhibitionPeriod2);
            exhibitionPeriod1.Should().NotBe(exhibitionPeriod3);
        }
    }
}