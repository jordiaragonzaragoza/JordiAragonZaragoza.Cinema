﻿namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Movie.Domain
{
    using System;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;
    using Xunit;

    public sealed class RuntimeTests
    {
        [Fact]
        public void CreateRuntime_WhenHavingADefaultTimeSpan_ShouldThrowArgumentException()
        {
            // Arrange
            TimeSpan value = default;

            // Act
            Func<Runtime> runtime = () => Runtime.Create(value);

            // Assert
            runtime.Should().Throw<BusinessRuleValidationException>().WithMessage("The runtime must be greater than zero to be valid.");
        }

        [Fact]
        public void CreateRuntime_WhenHavingAValidTimeSpan_ShouldReturnRuntime()
        {
            // Arrange
            var value = TimeSpan.FromHours(2);

            // Act
            var runtime = Runtime.Create(value);

            // Assert
            runtime.Should().NotBeNull();
        }
    }
}