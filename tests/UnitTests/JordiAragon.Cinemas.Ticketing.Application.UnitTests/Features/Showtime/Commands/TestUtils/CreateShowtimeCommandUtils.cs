namespace JordiAragon.Cinemas.Ticketing.Application.UnitTests.Features.Showtime.Commands.TestUtils
{
    using System;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts.Showtime.Commands;
    using JordiAragon.Cinemas.Ticketing.Domain.UnitTests.TestUtils.Constants;

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