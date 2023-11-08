namespace JordiAragon.Cinema.Reservation.UnitTests.Movie.Domain
{
    using System;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
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

        [Fact]
        public void CreateMovieId_WhenHavingAValidGuid_ShouldReturnMovieId()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var movieId = MovieId.Create(id);

            // Assert
            movieId.Should().NotBeNull();
        }
    }
}