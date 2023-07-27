namespace JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants
{
    using System;
    using JordiAragon.Cinema.Domain.MovieAggregate;

    public static partial class Constants
    {
        public static class Movie
        {
            public const string Title = "Inception";
            public const string ImdbId = "tt1375666";
            public const string Stars = "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page, Ken Watanabe";
            public static readonly MovieId Id = MovieId.Create(Guid.NewGuid());
            public static readonly DateTime ReleaseDateOnUtc = new(2010, 01, 14);
        }
    }
}