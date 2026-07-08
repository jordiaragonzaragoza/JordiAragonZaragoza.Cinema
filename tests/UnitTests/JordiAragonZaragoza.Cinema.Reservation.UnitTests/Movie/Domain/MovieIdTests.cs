namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Movie.Domain
{
    using System;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using Xunit;

    public sealed class MovieIdTests
    {
        [Fact]
        public void CreateMovieId_WhenHavingAnEmptyGuid_ShouldThrowArgumentException()
        {
            // Arrange
            var id = Guid.Empty;

            // Act
            Func<MovieId> movieId = () => new MovieId(id);

            // Assert
            movieId.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void ImplicitConversion_WhenHavingAMovieId_ShouldReturnGuid()
        {
            // Arrange
            var value = Guid.CreateVersion7();
            var movieId = new MovieId(value);

            // Act
            Guid result = movieId;

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void ToString_ShouldReturnStringRepresentationOfMovieId()
        {
            // Arrange
            var value = Guid.CreateVersion7();
            var movieId = new MovieId(value);

            // Act
            var result = movieId.ToString();

            // Assert
            result.Should().Be(value.ToString());
        }

        [Fact]
        public void Equality_Checks_ShouldWorkAsExpected()
        {
            // Arrange
            var value1 = Guid.CreateVersion7();
            var value2 = Guid.CreateVersion7();

            var movieId1 = new MovieId(value1);
            var movieId2 = new MovieId(value1);
            var movieId3 = new MovieId(value2);

            // Act & Assert
            movieId1.Should().Be(movieId2);
            movieId1.Should().NotBe(movieId3);
        }
    }
}