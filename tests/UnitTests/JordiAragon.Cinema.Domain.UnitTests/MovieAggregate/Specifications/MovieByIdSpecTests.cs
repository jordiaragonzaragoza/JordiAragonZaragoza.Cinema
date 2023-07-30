namespace JordiAragon.Cinema.Domain.UnitTests.MovieAggregate.Specifications
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using JordiAragon.Cinema.Domain.MovieAggregate.Specifications;
    using JordiAragon.Cinema.Domain.UnitTests.Features.Movie.TestUtils;
    using JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants;
    using Xunit;

    public class MovieByIdSpecTests
    {
        [Fact]
        public void FindMovieByIdSpec_WhenHavingAValidMovieId_ShouldContainTheMovie()
        {
            // Arrange
            var id = MovieId.Create(Guid.NewGuid());

            var movie1 = Movie.Create(
                id,
                Constants.Movie.Title,
                Constants.Movie.ImdbId,
                Constants.Movie.ReleaseDateOnUtc,
                Constants.Movie.Stars);

            var movie2 = CreateMovieUtils.Create();

            var movies = new List<Movie>() { movie1, movie2 };

            var specification = new MovieByIdSpec(id);

            // Act
            var evaluatedList = specification.Evaluate(movies);

            // Assert
            evaluatedList.Should()
                         .ContainSingle(c => c == movie1)
                         .And.NotContain(c => c == movie2);
        }

        [Fact]
        public void FindMovieByIdSpec_WhenHavingAnInvalidMovieId_ShouldNotContainTheMovie()
        {
            // Arrange
            var movie1 = CreateMovieUtils.Create();

            var movies = new List<Movie>() { movie1 };

            var specification = new MovieByIdSpec(MovieId.Create(Guid.NewGuid()));

            // Act
            var evaluatedList = specification.Evaluate(movies);

            // Assert
            evaluatedList.Should().BeEmpty();
            evaluatedList.Should()
                         .NotContain(c => c == movie1);
        }

        [Fact]
        public void FindMovieByIdSpec_WhenHavingANullMovieId_ShouldThrowArgumentNullException()
        {
            // Arrange
            MovieId movieId = null;

            // Act
            Func<MovieByIdSpec> movie = () => new MovieByIdSpec(movieId);

            // Assert
            movie.Should().Throw<ArgumentNullException>();
        }
    }
}