namespace JordiAragon.Cinema.Domain.UnitTests.MovieAggregate
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinema.Domain.MovieAggregate;
    using Xunit;

    public class MovieIdTests
    {
        [Fact]
        public void CreateMovieId_WhenHavingAnEmptyGuid_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<MovieId> movieId = () => MovieId.Create(id);

            // Assert
            movieId.Should().Throw<ArgumentException>();
        }
    }
}