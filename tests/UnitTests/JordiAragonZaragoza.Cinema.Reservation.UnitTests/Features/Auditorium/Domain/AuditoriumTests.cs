namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain.Events;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using JordiAragonZaragoza.SharedKernel.Domain.Exceptions;
    using Xunit;

    public sealed class AuditoriumTests
    {
        public static IEnumerable<object[]> InvalidArgumentsCreateAuditorium()
        {
            var id = Constants.Auditorium.Id;
            string name = Constants.Auditorium.Name;
            var rows = Constants.Auditorium.Rows;
            var seatsPerRow = Constants.Auditorium.SeatsPerRow;

            var idValues = new object[] { default!, id };
            var nameValues = new object[] { default!, string.Empty, " ", name };
            var rowsValues = new object[] { default!, rows };
            var seatsPerRowValues = new object[] { default!, seatsPerRow };

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
                                rowsValue != null && rowsValue.Equals(rows) &&
                                seatsPerRowValue != null && seatsPerRowValue.Equals(seatsPerRow))
                            {
                                continue;
                            }

                            yield return new object[] { idValue!, nameValue!, rowsValue!, seatsPerRowValue! };
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
            var rows = Constants.Auditorium.Rows;
            var seatsPerRow = Constants.Auditorium.SeatsPerRow;

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
            Rows rows,
            SeatsPerRow seatsPerRow)
        {
            // Act
            Func<Auditorium> auditorium = () => Auditorium.Create(id, name, rows, seatsPerRow);

            // Assert
            auditorium.Should().Throw<Exception>();
        }

        [Fact]
        public void RemoveAuditorium_WhenHavingValidArguments_ShouldAddAuditoriumRemovedEvent()
        {
            // Arrange.
            var auditorium = CreateAuditoriumUtils.Create();

            // Act.
            auditorium.Remove();

            auditorium.Events.Should()
                              .ContainSingle(x => x is AuditoriumRemovedEvent)
                              .Which.Should().BeOfType<AuditoriumRemovedEvent>()
                              .Which.Should().Match<AuditoriumRemovedEvent>(e => e.AggregateId == auditorium.Id);
        }

        [Fact]
        public void AddActiveShowtimeToAuditorium_WhenShowtimeIdIsValid_ShouldAddShowtimeIdAndAddActiveShowtimeAddedEvent()
        {
            // Arrange.
            var auditorium = CreateAuditoriumUtils.Create();
            var showtimeId = Constants.Showtime.Id;

            // Act.
            auditorium.AddActiveShowtime(showtimeId);

            // Assert.
            auditorium.ActiveShowtimes.Should()
                          .Contain(showtimeId)
                          .And
                          .HaveCount(1);

            auditorium.Events.Should()
                              .ContainSingle(x => x is ActiveShowtimeAddedEvent)
                              .Which.Should().BeOfType<ActiveShowtimeAddedEvent>()
                              .Which.Should().Match<ActiveShowtimeAddedEvent>(e =>
                                                                            e.AggregateId == auditorium.Id &&
                                                                            e.ShowtimeId == showtimeId);
        }

        [Fact]
        public void AddActiveShowtimeToAuditorium_WhenShowtimeIdIsInvalid_ShouldThrowArgumentNullException()
        {
            // Arrange.
            var auditorium = CreateAuditoriumUtils.Create();
            ShowtimeId showtimeId = null!;

            // Act.
            Action addShowtime = () => auditorium.AddActiveShowtime(showtimeId);

            // Assert.
            addShowtime.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveActiveShowtimeToAuditorium_WhenShowtimeIdIsValid_ShouldAddShowtimeIdAndAddActiveShowtimeRemovedEvent()
        {
            // Arrange.
            var auditorium = CreateAuditoriumUtils.Create();
            var showtimeId = Constants.Showtime.Id;
            auditorium.AddActiveShowtime(showtimeId);

            // Act.
            auditorium.RemoveActiveShowtime(showtimeId);

            // Assert.
            auditorium.ActiveShowtimes.Should()
                          .NotContain(showtimeId)
                          .And
                          .HaveCount(0);

            auditorium.Events.Should()
                              .ContainSingle(x => x is ActiveShowtimeRemovedEvent)
                              .Which.Should().BeOfType<ActiveShowtimeRemovedEvent>()
                              .Which.Should().Match<ActiveShowtimeRemovedEvent>(e =>
                                                                            e.AggregateId == auditorium.Id &&
                                                                            e.ShowtimeId == showtimeId);
        }

        [Fact]
        public void RemoveShowtimeToAuditorium_WhenShowtimeIdIsInvalid_ShouldThrowArgumentNullException()
        {
            // Arrange.
            var auditorium = CreateAuditoriumUtils.Create();
            ShowtimeId showtimeId = default!;

            // Act.
            Action removeShowtime = () => auditorium.RemoveActiveShowtime(showtimeId);

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
            Action removeShowtime = () => auditorium.RemoveActiveShowtime(showtimeId);

            // Assert.
            removeShowtime.Should().Throw<NotFoundException>();
        }
    }
}