namespace JordiAragon.Cinema.Domain.UnitTests.Features.Showtime.TestUtils
{
    using JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants;
    using JordiAragon.Cinema.Domain.ShowtimeAggregate;

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