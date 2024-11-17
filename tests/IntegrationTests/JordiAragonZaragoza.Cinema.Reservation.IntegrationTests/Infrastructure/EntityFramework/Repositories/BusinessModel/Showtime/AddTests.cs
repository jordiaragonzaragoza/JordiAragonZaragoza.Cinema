namespace JordiAragonZaragoza.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Repositories.BusinessModel.Showtime
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain;
    using Microsoft.EntityFrameworkCore;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class AddTests : BaseEntityFrameworkIntegrationTests
    {
        public AddTests(
            IntegrationTestsFixture fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task AddAsync_WhenHavingAnUnexistingShowtime_ShouldAddTheShowtime()
        {
            // Arrange
            var newShowtime = Showtime.Schedule(
                new ShowtimeId(Guid.NewGuid()),
                new MovieId(Constants.Movie.Id),
                DateTimeOffset.UtcNow.AddDays(1),
                new AuditoriumId(Constants.Auditorium.Id));

            var repository = this.GetBusinessModelRepository<Showtime, ShowtimeId>();

            // Act
            await repository.AddAsync(newShowtime);

            // Assert
            var result = await repository.ListAsync();

            result.Should()
                .NotBeNullOrEmpty()
                .And
                .Contain(newShowtime);
        }

        [Fact]
        public async Task AddAsync_WhenHavingAnExistingShowtime_ShouldThrowDbUpdateException()
        {
            // Arrange
            var existingShowtime = Showtime.Schedule(
                new ShowtimeId(Guid.NewGuid()),
                new MovieId(Constants.Movie.Id),
                DateTimeOffset.UtcNow.AddDays(1),
                new AuditoriumId(Constants.Auditorium.Id));

            var repository = this.GetBusinessModelRepository<Showtime, ShowtimeId>();

            await repository.AddAsync(existingShowtime);

            // Act
            Func<Task> addAsync = async () => await repository.AddAsync(existingShowtime);

            // Assert
            await addAsync.Should().ThrowAsync<DbUpdateException>();
        }
    }
}