namespace JordiAragon.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain.Events;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain;
    using JordiAragon.SharedKernel.Domain.Exceptions;
    using Xunit;

    public sealed class AuditoriumTests
    {
        public static IEnumerable<object[]> InvalidArgumentsCreateAuditorium()
        {
            var id = Constants.Auditorium.Id;
            string name = Constants.Auditorium.Name;
            ushort rows = 10;
            ushort seatsPerRow = 10;

            var idValues = new object[] { null, id };
            var nameValues = new object[] { null, string.Empty, " ", name };
            var rowsValues = new object[] { 0, rows };
            var seatsPerRowValues = new object[] { 0, seatsPerRow };

            foreach (var idValue in idValues)
            {
                foreach (var nameValue in nameValues)
                {
                    foreach (var rowsValue in rowsValues)
                    {
                        foreach (var seatsPerRowValue in seatsPerRowValues)
                        {
                            if (idValue != null && idValue.Equals(id) &&
                                nameValue != null && nameValue.Equals(name) &&
                                rowsValue.Equals(rows) &&
                                seatsPerRowValue.Equals(seatsPerRow))
                            {
                                continue;
                            }

                            yield return new object[] { idValue, nameValue, rowsValue, seatsPerRowValue };
                        }
                    }
                }
            }
        }

        [Fact]
        public void CreateAuditorium_WhenHavingCorrectArguments_ShouldCreateAuditoriumAndAddAuditoriumCreatedEvent()
        {
            // Arrange
            var id = Constants.Auditorium.Id;
            string name = Constants.Auditorium.Name;
            ushort rows = Constants.Auditorium.Rows;
            ushort seatsPerRow = Constants.Auditorium.SeatsPerRow;

            // Act
            var auditorium = Auditorium.Create(id, name, rows, seatsPerRow);

            // Assert
            auditorium.Should().NotBeNull();
            auditorium.Id.Should().Be(id);
            auditorium.Rows.Should().Be(rows);
            auditorium.SeatsPerRow.Should().Be(seatsPerRow);
            auditorium.Seats.Should().HaveCount(rows * seatsPerRow);

            auditorium.Events.Should()
                              .ContainSingle(x => x is AuditoriumCreatedEvent)
                              .Which.Should().BeOfType<AuditoriumCreatedEvent>()
                              .Which.Should().Match<AuditoriumCreatedEvent>(e =>
                                                                            e.AggregateId == id &&
                                                                            e.Rows == rows &&
                                                                            e.SeatsPerRow == seatsPerRow);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateAuditorium))]
        public void CreateAuditorium_WhenHavingInCorrectRowsSeatsArguments_ShouldThrowInvalidAggregateStateException(
            AuditoriumId id,
            string name,
            ushort rows,
            ushort seatsPerRow)
        {
            // Act
            Func<Auditorium> auditorium = () => Auditorium.Create(id, name, rows, seatsPerRow);

            // Assert
            auditorium.Should().Throw<Exception>();
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
                              .ContainSingle(x => x is ShowtimeAddedEvent)
                              .Which.Should().BeOfType<ShowtimeAddedEvent>()
                              .Which.Should().Match<ShowtimeAddedEvent>(e =>
                                                                            e.AggregateId == auditorium.Id &&
                                                                            e.ShowtimeId == showtimeId);
        }

        [Fact]
        public void AddShowtimeToAuditorium_WhenShowtimeIdIsInvalid_ShouldThrowArgumentNullException()
        {
            // Arrange.
            var auditorium = CreateAuditoriumUtils.Create();
            ShowtimeId showtimeId = null;

            // Act.
            Action addShowtime = () => auditorium.AddShowtime(showtimeId);

            // Assert.
            addShowtime.Should().Throw<ArgumentNullException>();
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
                              .ContainSingle(x => x is ShowtimeRemovedEvent)
                              .Which.Should().BeOfType<ShowtimeRemovedEvent>()
                              .Which.Should().Match<ShowtimeRemovedEvent>(e =>
                                                                            e.AggregateId == auditorium.Id &&
                                                                            e.ShowtimeId == showtimeId);
        }

        [Fact]
        public void RemoveShowtimeToAuditorium_WhenShowtimeIdIsInvalid_ShouldThrowArgumentNullException()
        {
            // Arrange.
            var auditorium = CreateAuditoriumUtils.Create();
            ShowtimeId showtimeId = null;

            // Act.
            Action removeShowtime = () => auditorium.RemoveShowtime(showtimeId);

            // Assert.
            removeShowtime.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveShowtimeToAuditorium_WhenShowtimeIdIsNotPresent_ShouldThrowNotFoundException()
        {
            // Arrange
            var auditorium = CreateAuditoriumUtils.Create();
            var showtimeId = Constants.Showtime.Id;

            // Act.
            Action removeShowtime = () => auditorium.RemoveShowtime(showtimeId);

            // Assert.
            removeShowtime.Should().Throw<NotFoundException>();
        }
    }
}