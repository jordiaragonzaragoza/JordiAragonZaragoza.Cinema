namespace JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Application
{
    using System;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.UnitTests.TestUtils.Domain;

    public static class CreateShowtimeCommandUtils
    {
        public static CreateShowtimeCommand CreateCommand()
            => new(Constants.Auditorium.Id, Constants.Movie.Id, DateTime.UtcNow.AddYears(1));
    }
}