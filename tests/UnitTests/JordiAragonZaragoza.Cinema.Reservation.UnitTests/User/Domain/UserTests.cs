namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.User.Domain
{
    using System;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain.Events;
    using Xunit;

    public class UserTests
    {
        [Fact]
        public void CreateUser_WhenHavingInCorrectArguments_ShouldThrowArgumentNullException()
        {
            // Arrange
            UserId id = null!;

            // Act
            Func<User> user = () => User.Create(id);

            // Assert
            user.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CreateUser_WhenHavingCorrectArguments_ShouldCreateUserAndAddUserCreatedEvent()
        {
            // Arrange
            var id = Constants.User.Id;

            // Act
            var user = User.Create(id);

            // Assert
            user.Should().NotBeNull();
            user.Id.Should().Be(id);

            user.Events.Should()
                              .ContainSingle(x => x is UserCreatedEvent)
                              .Which.Should().BeOfType<UserCreatedEvent>()
                              .Which.Should().Match<UserCreatedEvent>(e => e.AggregateId == id);
        }

        [Fact]
        public void RemoveUser_WhenHavingValidArguments_ShouldAddUserRemovedEvent()
        {
            // Arrange.
            var user = CreateUserUtils.Create();

            // Act.
            user.Remove();

            user.Events.Should()
                              .ContainSingle(x => x is UserRemovedEvent)
                              .Which.Should().BeOfType<UserRemovedEvent>()
                              .Which.Should().Match<UserRemovedEvent>(e => e.AggregateId == user.Id);
        }
    }
}