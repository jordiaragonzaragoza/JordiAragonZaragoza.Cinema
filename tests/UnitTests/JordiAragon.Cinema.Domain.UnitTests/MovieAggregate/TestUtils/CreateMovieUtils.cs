namespace JordiAragon.Cinema.Domain.UnitTests.Features.Movie.TestUtils
{
    using JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants;
    using JordiAragon.Cinema.Domain.MovieAggregate;

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