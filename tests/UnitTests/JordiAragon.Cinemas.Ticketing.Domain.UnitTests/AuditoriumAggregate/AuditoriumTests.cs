namespace JordiAragon.Cinemas.Ticketing.Domain.UnitTests.AuditoriumAggregate
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Domain.AuditoriumAggregate;
    using JordiAragon.Cinemas.Ticketing.Domain.AuditoriumAggregate.Events;
    using JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate;
    using JordiAragon.Cinemas.Ticketing.Domain.UnitTests.Features.Auditorium.TestUtils;
    using JordiAragon.Cinemas.Ticketing.Domain.UnitTests.TestUtils.Constants;
    using JordiAragon.SharedKernel.Domain.Exceptions;
    using Xunit;

    public class AuditoriumTests
    {
        public static IEnumerable<object[]> InvalidArgumentsCreateAuditorium()
        {
            var argument1Values = new object[] { 10, -1, 0 };
            var argument2Values = new object[] { 10, -1, 0 };

            foreach (var arg1 in argument1Values)
            {
                foreach (var arg2 in argument2Values)
                {
                    if (arg1.Equals(10) &&
                        arg2.Equals(10))
                    {
                        continue;
                    }

                    yield return new object[] { arg1, arg2 };
                }
            }
        }

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
        [MemberData(nameof(InvalidArgumentsCreateAuditorium))]
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
                              .ContainSingle(x => x.GetType() == typeof(ShowtimeRemovedEvent))
                              .Which.Should().BeOfType<ShowtimeRemovedEvent>()
                              .Which.Should().Match<ShowtimeRemovedEvent>(e =>
                                                                            e.AuditoriumId == auditorium.Id &&
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