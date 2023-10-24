namespace JordiAragon.Cinemas.Ticketing.UnitTests.Auditorium.Domain
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Auditorium.Domain;
    using Xunit;

    public class AuditoriumIdTests
    {
        [Fact]
        public void CreateAuditoriumId_WhenHavingAnEmptyGuid_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<AuditoriumId> auditoriumId = () => AuditoriumId.Create(id);

            // Assert
            auditoriumId.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateAuditoriumId_WhenHavingAValidGuid_ShouldReturnAuditoriumId()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var auditoriumId = AuditoriumId.Create(id);

            // Assert
            auditoriumId.Should().NotBeNull();
        }
    }
}