namespace JordiAragon.Cinemas.Ticketing.UnitTests.TestUtils.Application
{
    using System;
    using JordiAragon.Cinemas.Ticketing.Showtime.Application.Contracts.Commands;
    using JordiAragon.Cinemas.Ticketing.UnitTests.TestUtils.Domain;

    public static class CreateShowtimeCommandUtils
    {
        public static CreateShowtimeCommand CreateCommand()
            => new()
            {
                AuditoriumId = Constants.Auditorium.Id,
                MovieId = Constants.Movie.Id,
                SessionDateOnUtc = DateTime.UtcNow.AddYears(1),
            };
    }
}