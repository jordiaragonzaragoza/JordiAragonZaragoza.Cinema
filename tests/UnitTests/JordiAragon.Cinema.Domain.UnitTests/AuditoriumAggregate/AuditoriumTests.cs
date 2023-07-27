namespace JordiAragon.Cinema.Domain.UnitTests.AuditoriumAggregate
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Events;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;
    using JordiAragon.Cinema.Domain.UnitTests.Features.Auditorium.TestUtils;
    using Xunit;

    public class AuditoriumTests
    {
        [Fact]
        public void CreateAuditorium_WhenHavingCorrectArguments_ShouldCreateAuditoriumAndAddAuditoriumCreatedEvent()
        {
            // Arrange
            var id = AuditoriumId.Create(Guid.NewGuid());
            short rows = 10;
            short seatsPerRow = 10;

            // Act
            var auditorium = Auditorium.Create(id, rows, seatsPerRow);

            // Assert
            auditorium.Should().NotBeNull();
            auditorium.Rows.Should().Be(rows);
            auditorium.SeatsPerRow.Should().Be(seatsPerRow);
            auditorium.Id.Should().Be(id);

            auditorium.Events.Should()
                              .ContainSingle(x => x.GetType() == typeof(AuditoriumCreatedEvent))
                              .Which.Should().BeOfType<AuditoriumCreatedEvent>()
                              .Which.Should().Match<AuditoriumCreatedEvent>(e =>
                                                                            e.AuditoriumId == id &&
                                                                            e.Rows == rows &&
                                                                            e.SeatsPerRow == seatsPerRow);
        }

        [Fact]
        public void AddShowtimeToAuditorium_WhenShowtimeIdIsValid_ShouldAddShowtimeIdAndAddShowtimeAddedEvent()
        {
            // Arrange.
            var auditorium = CreateAuditoriumUtils.Create();
            var showtimeId = ShowtimeId.Create(Guid.NewGuid());

            // Act.
            auditorium.AddShowtime(showtimeId);

            // Assert.
            auditorium.Showtimes.Should()
                          .Contain(showtimeId)
                          .And
                          .HaveCount(1);

            auditorium.Events.Should()
                              .ContainSingle(x => x.GetType() == typeof(ShowtimeAddedEvent))
                              .Which.Should().BeOfType<ShowtimeAddedEvent>()
                              .Which.Should().Match<ShowtimeAddedEvent>(e =>
                                                                            e.AuditoriumId == auditorium.Id &&
                                                                            e.ShowtimeId == showtimeId);
        }
    }
}