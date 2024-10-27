namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using Xunit;

    public sealed class SeatTests
    {
        public static IEnumerable<object[]> InvalidArgumentsCreateSeat()
        {
            var argument1Values = new object[] { default!, Constants.Seat.Id };
            var argument2Values = new object[] { default!, Constants.Seat.Row };
            var argument3Values = new object[] { default!, Constants.Seat.SeatNumber };

            foreach (var arg1 in argument1Values)
            {
                foreach (var arg2 in argument2Values)
                {
                    foreach (var arg3 in argument3Values)
                    {
                        if (arg1 != null && arg1.Equals(Constants.Seat.Id) &&
                            arg2 != null && arg2.Equals(Constants.Seat.Row) &&
                            arg3 != null && arg3.Equals(Constants.Seat.SeatNumber))
                        {
                            continue;
                        }

                        yield return new object[] { arg1!, arg2!, arg3! };
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateSeat))]
        public void CreateSeat_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            SeatId seatId,
            Row row,
            SeatNumber seatNumber)
        {
            // Act
            Func<Seat> createSeat = () => Seat.Create(seatId, row, seatNumber);

            // Assert
            createSeat.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateSeat_WhenHavingValidArguments_ShouldCreateSeat()
        {
            SeatId seatId = Constants.Seat.Id;
            Row row = Constants.Seat.Row;
            SeatNumber seatNumber = Constants.Seat.SeatNumber;

            // Act
            var seat = Seat.Create(seatId, row, seatNumber);

            // Assert
            seat.Should().NotBeNull();
            seat.Id.Should().Be(seatId);
            seat.Row.Should().Be(row);
            seat.SeatNumber.Should().Be(seatNumber);
        }
    }
}