namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Movie.Domain.Rules
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain.Rules;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using Xunit;

    public sealed class MinimumRuntimeTests
    {
        [Fact]
        public void IsBroken_WhenHavingDefaultValue_ShouldBeFalse()
        {
            // Arrange
            TimeSpan value = default;

            // Act
            var rule = new MinimumRuntimeRule(value);

            // Assert
            rule.IsBroken().Should().Be(true);
        }
    }
}