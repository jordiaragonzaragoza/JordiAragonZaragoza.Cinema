namespace JordiAragon.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Repositories.BusinessModel.Showtime
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class GetTests : BaseEntityFrameworkIntegrationTests
    {
        public GetTests(
            IntegrationTestsFixture fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetByIdAsync_WhenHavingAnExistingShowtime_ShouldReturnTheShowtime()
        {
            // Arrange
            var newShowtime = Showtime.Schedule(
                ShowtimeId.Create(Guid.NewGuid()),
                MovieId.Create(SeedData.ExampleMovie.Id),
                DateTimeOffset.UtcNow.AddDays(1),
                AuditoriumId.Create(SeedData.ExampleAuditorium.Id));

            var repository = this.GetBusinessModelRepository<Showtime, ShowtimeId>();

            await repository.AddAsync(newShowtime);

            // Act
            var result = await repository.GetByIdAsync(ShowtimeId.Create(newShowtime.Id));

            result.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(newShowtime);
        }

        [Fact]
        public async Task GetByIdAsync_WhenHavingAnUnExistingShowtime_ShouldThrowDbUpdateException()
        {
            // Arrange
            var repository = this.GetBusinessModelRepository<Showtime, ShowtimeId>();

            // Act
            var result = await repository.GetByIdAsync(ShowtimeId.Create(Guid.NewGuid()));

            // Assert
            result.Should()
                .BeNull();
        }
    }
}