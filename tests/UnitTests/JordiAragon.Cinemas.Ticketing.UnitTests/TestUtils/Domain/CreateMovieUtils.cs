namespace JordiAragon.Cinemas.Ticketing.UnitTests.TestUtils.Domain
{
    using JordiAragon.Cinemas.Ticketing.Movie.Domain;

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