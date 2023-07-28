namespace JordiAragon.Cinema.Domain.UnitTests.AuditoriumAggregate
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Events;
    using JordiAragon.Cinema.Domain.UnitTests.Features.Auditorium.TestUtils;
    using JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants;
    using JordiAragon.SharedKernel.Domain.Exceptions;
    using Xunit;

    public class AuditoriumTests
    {
        [Fact]
        public void CreateAuditorium_WhenHavingCorrectArguments_ShouldCreateAuditoriumAndAddAuditoriumCreatedEvent()
        {
            // Arrange
            var id = Constants.Auditorium.Id;
            short rows = Constants.Auditorium.Rows;
            short seatsPerRow = Constants.Auditorium.SeatsPerRow;

            // Act
            var auditorium = Auditorium.Create(id, rows, seatsPerRow);

            // Assert
            auditorium.Should().NotBeNull();
            auditorium.Id.Should().Be(id);
            auditorium.Rows.Should().Be(rows);
            auditorium.SeatsPerRow.Should().Be(seatsPerRow);
            auditorium.Seats.Should().HaveCount(rows * seatsPerRow);

            auditorium.Events.Should()
                              .ContainSingle(x => x.GetType() == typeof(AuditoriumCreatedEvent))
                              .Which.Should().BeOfType<AuditoriumCreatedEvent>()
                              .Which.Should().Match<AuditoriumCreatedEvent>(e =>
                                                                            e.AuditoriumId == id &&
                                                                            e.Rows == rows &&
                                                                            e.SeatsPerRow == seatsPerRow);
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(-1, -1)]
        [InlineData(0, 0)]
        public void CreateAuditorium_WhenHavingInCorrectRowsSeatsArguments_ShouldThrowInvalidAggregateStateException(short rows, short seatsPerRow)
        {
            // Arrange
            var id = Constants.Auditorium.Id;

            // Act
            Func<Auditorium> auditorium = () => Auditorium.Create(id, rows, seatsPerRow);

            // Assert
            auditorium.Should().Throw<InvalidAggregateStateException>();
        }

        [Fact]
        public void CreateAuditorium_WhenHavingInCorrectAuditoriumIdArgument_ShouldThrowArgumentNullException()
        {
            // Arrange
            AuditoriumId id = null;
            short rows = Constants.Auditorium.Rows;
            short seatsPerRow = Constants.Auditorium.SeatsPerRow;

            // Act
            Func<Auditorium> auditorium = () => Auditorium.Create(id, rows, seatsPerRow);

            // Assert
            auditorium.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddShowtimeToAuditorium_WhenShowtimeIdIsValid_ShouldAddShowtimeIdAndAddShowtimeAddedEvent()
        {
            // Arrange.
            var auditorium = CreateAuditoriumUtils.Create();
            var showtimeId = Constants.Showtime.Id;

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

        [Fact]
        public void RemoveShowtimeToAuditorium_WhenShowtimeIdIsValid_ShouldAddShowtimeIdAndAddShowtimeRemovedEvent()
        {
            // Arrange.
            var auditorium = CreateAuditoriumUtils.Create();
            var showtimeId = Constants.Showtime.Id;
            auditorium.AddShowtime(showtimeId);

            // Act.
            auditorium.RemoveShowtime(showtimeId);

            // Assert.
            auditorium.Showtimes.Should()
                          .NotContain(showtimeId)
                          .And
                          .HaveCount(0);

            auditorium.Events.Should()
                              .ContainSingle(x => x.GetType() == typeof(ShowtimeRemovedEvent))
                              .Which.Should().BeOfType<ShowtimeRemovedEvent>()
                              .Which.Should().Match<ShowtimeRemovedEvent>(e =>
                                                                            e.AuditoriumId == auditorium.Id &&
                                                                            e.ShowtimeId == showtimeId);
        }
    }
}