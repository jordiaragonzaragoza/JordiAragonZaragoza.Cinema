namespace JordiAragon.Cinemas.Reservation.UnitTests.TestUtils.Domain
{
    using JordiAragon.Cinemas.Reservation.Movie.Domain;

    public static class CreateMovieUtils
    {
        public static Movie Create()
            => Movie.Create(
                Constants.Movie.Id,
                Constants.Movie.Title,
                Constants.Movie.ImdbId,
                Constants.Movie.ReleaseDateOnUtc,
                Constants.Movie.Stars);
    }
}