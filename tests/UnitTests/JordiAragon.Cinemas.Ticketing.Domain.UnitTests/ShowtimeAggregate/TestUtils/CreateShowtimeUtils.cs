namespace JordiAragon.Cinemas.Ticketing.Domain.UnitTests.Features.Showtime.TestUtils
{
    using JordiAragon.Cinemas.Ticketing.Domain.UnitTests.TestUtils.Constants;
    using JordiAragon.Cinemas.Ticketing.Domain.ShowtimeAggregate;

    public static class CreateShowtimeUtils
    {
        public static Showtime Create()
            => Showtime.Create(
                Constants.Showtime.Id,
                Constants.Showtime.MovieId,
                Constants.Showtime.SessionDateOnUtc,
                Constants.Showtime.AuditoriumId);
    }
}