namespace JordiAragon.Cinema.Application.UnitTests.Features.Showtime.Commands.TestUtils
{
    using System;
    using JordiAragon.Cinema.Application.Contracts.Features.Showtime.Commands;
    using JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants;

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