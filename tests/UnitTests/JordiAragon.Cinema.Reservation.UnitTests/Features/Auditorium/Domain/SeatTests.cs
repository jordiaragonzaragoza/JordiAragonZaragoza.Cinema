namespace JordiAragon.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain;
    using Xunit;

    public class SeatTests
    {
        public static IEnumerable<object[]> InvalidArgumentsCreateSeat()
        {
            var argument1Values = new object[] { null, Constants.Seat.Id };
            var argument2Values = new object[] { 0, -1, 10 };
            var argument3Values = new object[] { 0, -1, 10 };

            foreach (var arg1 in argument1Values)
            {
                foreach (var arg2 in argument2Values)
                {
                    foreach (var arg3 in argument3Values)
                    {
                        if (arg1 != null && arg1.Equals(Constants.Seat.Id) &&
                            arg2.Equals(10) &&
                            arg3.Equals(10))
                        {
                            continue;
                        }

                        yield return new object[] { arg1, arg2, arg3 };
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsCreateSeat))]
        public void CreateSeat_WhenHavingInvalidArguments_ShouldThrowArgumentException(
            SeatId seatId,
            short row,
            short seatNumber)
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
            short row = Constants.Seat.Row;
            short seatNumber = Constants.Seat.SeatNumber;

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