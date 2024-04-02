namespace JordiAragon.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Repositories.ReadModel.Showtime
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
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

            // Act
            var result = await repository.GetByIdAsync(SeedData.ExampleShowtimeReadModel.Id);

            result.Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(SeedData.ExampleShowtimeReadModel);
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
    }
}