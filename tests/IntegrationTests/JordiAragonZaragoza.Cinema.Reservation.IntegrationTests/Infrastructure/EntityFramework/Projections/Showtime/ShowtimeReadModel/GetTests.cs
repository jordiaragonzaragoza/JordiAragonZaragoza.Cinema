namespace JordiAragonZaragoza.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Projections.Showtime.ShowtimeReadModel
{
    using System;
    using System.Threading.Tasks;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Projections;
    using JordiAragonZaragoza.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
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
        public async Task GetByIdAsync_WhenHavingAnExistingShowtimeReadModel_ShouldReturnTheShowtimeReadModel()
        {
            // Arrange
            var repository = this.GetReadModelRepository<ShowtimeReadModel>();

            var existingShowtimeReadModel = await AddNewShowtimeReadModelAsync(repository);

            // Act
            var result = await repository.GetByIdAsync(existingShowtimeReadModel.Id);

            result.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(existingShowtimeReadModel);
        }

        [Fact]
        public async Task GetByIdAsync_WhenHavingAnUnExistingShowtime_ShouldReturnNull()
        {
            // Arrange
            var repository = this.GetReadModelRepository<ShowtimeReadModel>();

            // Act
            var result = await repository.GetByIdAsync(Guid.CreateVersion7());

            // Assert
            result.Should()
                .BeNull();
        }

        private static async Task<ShowtimeReadModel> AddNewShowtimeReadModelAsync(
            ReservationReadModelRepository<ShowtimeReadModel> repository)
        {
            var newShowtime = new ShowtimeReadModel(
                Guid.CreateVersion7(),
                DateTimeOffset.UtcNow,
                Guid.CreateVersion7(),
                "Some title",
                TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28),
                Guid.CreateVersion7(),
                "Some auditorium");

            await repository.AddAsync(newShowtime);

            return newShowtime;
        }
    }
}