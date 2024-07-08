namespace JordiAragon.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Repositories.ReadModel.Showtime
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.ReadModel;
    using JordiAragon.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
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
            var newShowtime = new ShowtimeReadModel(
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                Guid.NewGuid(),
                "Some title",
                TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28),
                Guid.NewGuid(),
                "Some auditorium");

            var repository = this.GetReadModelRepository<ShowtimeReadModel>();

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
            var repository = this.GetReadModelRepository<ShowtimeReadModel>();

            var existingShowtimeReadModel = await AddNewShowtimeReadModelAsync(repository);

            // Act
            Func<Task> addAsync = async () => await repository.AddAsync(existingShowtimeReadModel);

            // Assert
            await addAsync.Should().ThrowAsync<DbUpdateException>();
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