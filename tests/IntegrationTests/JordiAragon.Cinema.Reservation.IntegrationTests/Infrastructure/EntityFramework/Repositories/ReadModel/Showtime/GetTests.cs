namespace JordiAragon.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Repositories.ReadModel.Showtime
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.ReadModel;
    using JordiAragon.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
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
        public async Task GetByIdAsync_WhenHavingAnUnExistingShowtime_ShouldThrowDbUpdateException()
        {
            // Arrange
            var repository = this.GetReadModelRepository<ShowtimeReadModel>();

            // Act
            var result = await repository.GetByIdAsync(Guid.NewGuid());

            // Assert
            result.Should()
                .BeNull();
        }

        private static async Task<ShowtimeReadModel> AddNewShowtimeReadModelAsync(
            ReservationReadModelRepository<ShowtimeReadModel> repository)
        {
            var newShowtime = new ShowtimeReadModel(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                Guid.NewGuid(),
                "Some title",
                TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28),
                Guid.NewGuid(),
                "Some auditorium");

            await repository.AddAsync(newShowtime);

            return newShowtime;
        }
    }
}