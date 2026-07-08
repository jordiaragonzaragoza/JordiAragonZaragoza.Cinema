namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using Xunit;

    public sealed class AuditoriumIdTests
    {
        [Fact]
        public void CreateAuditoriumId_WhenHavingAnEmptyGuid_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<AuditoriumId> auditoriumId = () => new AuditoriumId(id);

            // Assert
            auditoriumId.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ImplicitConversion_WhenHavingAAuditoriumId_ShouldReturnGuid()
        {
            // Arrange
            var value = Guid.CreateVersion7();
            var auditoriumId = new AuditoriumId(value);

            // Act
            Guid result = auditoriumId;

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void ToString_ShouldReturnStringRepresentationOfAuditoriumId()
        {
            // Arrange
            var value = Guid.CreateVersion7();
            var auditoriumId = new AuditoriumId(value);

            // Act
            var result = auditoriumId.ToString();

            // Assert
            result.Should().Be(value.ToString());
        }

        [Fact]
        public void Equality_Checks_ShouldWorkAsExpected()
        {
            // Arrange
            var value1 = Guid.CreateVersion7();
            var value2 = Guid.CreateVersion7();

            var auditoriumId1 = new AuditoriumId(value1);
            var auditoriumId2 = new AuditoriumId(value1);
            var auditoriumId3 = new AuditoriumId(value2);

            // Act & Assert
            auditoriumId1.Should().Be(auditoriumId2);
            auditoriumId1.Should().NotBe(auditoriumId3);
        }
    }
}