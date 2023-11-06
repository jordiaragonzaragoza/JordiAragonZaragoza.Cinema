namespace JordiAragon.Cinemas.Reservation.UnitTests.Showtime.Domain.Specifications
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragon.Cinemas.Reservation.Showtime.Domain;
    using JordiAragon.Cinemas.Reservation.Showtime.Domain.Specifications;
    using JordiAragon.Cinemas.Reservation.UnitTests.TestUtils.Domain;
    using Xunit;

    public class ShowtimeByIdSpecTests
    {
        [Fact]
        public void FindShowtimeByIdSpec_WhenHavingAValidShowtimeId_ShouldContainTheShowtime()
        {
            // Arrange
            var id = ShowtimeId.Create(Guid.NewGuid());

            var showtime1 = Showtime.Create(
                id,
                Constants.Showtime.MovieId,
                Constants.Showtime.SessionDateOnUtc,
                Constants.Showtime.AuditoriumId);

            var showtime2 = CreateShowtimeUtils.Create();

            var showtimes = new List<Showtime>() { showtime1, showtime2 };

            var specification = new ShowtimeByIdSpec(id);

            // Act
            var evaluatedList = specification.Evaluate(showtimes);

            // Assert
            evaluatedList.Should()
                         .ContainSingle(c => c == showtime1)
                         .And.NotContain(c => c == showtime2);
        }

        [Fact]
        public void FindShowtimeByIdSpec_WhenHavingAnInvalidShowtimeId_ShouldNotContainTheShowtime()
        {
            // Arrange
            var showtime1 = CreateShowtimeUtils.Create();

            var showtimes = new List<Showtime>() { showtime1 };

            var specification = new ShowtimeByIdSpec(ShowtimeId.Create(Guid.NewGuid()));

            // Act
            var evaluatedList = specification.Evaluate(showtimes);

            // Assert
            evaluatedList.Should().BeEmpty();
            evaluatedList.Should()
                         .NotContain(c => c == showtime1);
        }

        [Fact]
        public void FindShowtimeByIdSpec_WhenHavingANullShowtimeId_ShouldThrowArgumentNullException()
        {
            // Arrange
            ShowtimeId showtimeId = null;

            // Act
            Func<ShowtimeByIdSpec> showtime = () => new ShowtimeByIdSpec(showtimeId);

            // Assert
            showtime.Should().Throw<ArgumentNullException>();
        }
    }
}