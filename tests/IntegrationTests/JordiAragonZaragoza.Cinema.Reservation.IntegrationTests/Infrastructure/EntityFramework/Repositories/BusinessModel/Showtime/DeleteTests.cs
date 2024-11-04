namespace JordiAragonZaragoza.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Repositories.BusinessModel.Showtime
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragonZaragoza.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using Microsoft.EntityFrameworkCore;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class DeleteTests : BaseEntityFrameworkIntegrationTests
    {
        public DeleteTests(
            IntegrationTestsFixture fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task DeleteAsync_WhenHavingAnExistingShowtime_ShouldDeleteTheShowtime()
        {
            // Arrange
            var newShowtime = Showtime.Schedule(
                new ShowtimeId(Guid.NewGuid()),
                new MovieId(Constants.Movie.Id),
                DateTimeOffset.UtcNow.AddDays(1),
                new AuditoriumId(Constants.Auditorium.Id));

            var repository = this.GetBusinessModelRepository<Showtime, ShowtimeId>();

            await repository.AddAsync(newShowtime);

            // Act
            await repository.DeleteAsync(newShowtime);

            // Assert
            var result = await repository.ListAsync();

            result.Should()
                .NotContain(newShowtime);
        }

        [Fact]
        public async Task DeleteAsync_WhenHavingAnUnexistingShowtime_ShouldThrowDbUpdateException()
        {
            // Arrange
            var newShowtime = Showtime.Schedule(
                new ShowtimeId(Guid.NewGuid()),
                new MovieId(Constants.Movie.Id),
                DateTimeOffset.UtcNow.AddDays(1),
                new AuditoriumId(Constants.Auditorium.Id));

            var repository = this.GetBusinessModelRepository<Showtime, ShowtimeId>();

            // Act
            Func<Task> deleteAsync = async () => await repository.DeleteAsync(newShowtime);

            // Assert
            await deleteAsync.Should().ThrowAsync<DbUpdateConcurrencyException>();
        }
    }
}