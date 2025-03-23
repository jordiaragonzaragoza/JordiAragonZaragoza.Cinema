namespace JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Application
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;

    public static class ScheduleShowtimeCommandUtils
    {
        public static ScheduleShowtimeCommand CreateCommand()
            => new(Guid.NewGuid(), Constants.Auditorium.Id, Constants.Movie.Id, DateTimeOffset.UtcNow.AddYears(1));
    }
}