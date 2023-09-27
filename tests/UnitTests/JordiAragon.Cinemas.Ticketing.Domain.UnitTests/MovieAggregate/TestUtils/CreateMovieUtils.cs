namespace JordiAragon.Cinemas.Ticketing.Domain.UnitTests.Features.Movie.TestUtils
{
    using JordiAragon.Cinemas.Ticketing.Domain.UnitTests.TestUtils.Constants;
    using JordiAragon.Cinemas.Ticketing.Domain.MovieAggregate;

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