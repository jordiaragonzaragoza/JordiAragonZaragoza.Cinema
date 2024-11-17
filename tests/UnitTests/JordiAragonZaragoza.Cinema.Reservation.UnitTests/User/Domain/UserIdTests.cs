namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Domain
{
    using System;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using Xunit;

    public sealed class UserIdTests
    {
        [Fact]
        public void CreateUserId_WhenHavingAnEmptyGuid_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<UserId> userId = () => new UserId(id);

            // Assert
            userId.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ImplicitConversion_WhenHavingAUserId_ShouldReturnGuid()
        {
            // Arrange
            var value = Guid.NewGuid();
            var userId = new UserId(value);

            // Act
            Guid result = userId;

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void ToString_ShouldReturnStringRepresentationOfUserId()
        {
            // Arrange
            var value = Guid.NewGuid();
            var userId = new UserId(value);

            // Act
            var result = userId.ToString();

            // Assert
            result.Should().Be(value.ToString());
        }

        [Fact]
        public void Equality_Checks_ShouldWorkAsExpected()
        {
            // Arrange
            var value1 = Guid.NewGuid();
            var value2 = Guid.NewGuid();

            var userId1 = new UserId(value1);
            var userId2 = new UserId(value1);
            var userId3 = new UserId(value2);

            // Act & Assert
            userId1.Should().Be(userId2);
            userId1.Should().NotBe(userId3);
        }
    }
}