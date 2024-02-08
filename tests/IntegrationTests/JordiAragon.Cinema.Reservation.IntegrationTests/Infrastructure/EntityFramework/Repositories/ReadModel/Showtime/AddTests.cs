namespace JordiAragon.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Repositories.ReadModel.Showtime
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using Microsoft.EntityFrameworkCore;
    using Xunit;
    using Xunit.Abstractions;

    public class AddTests : BaseEntityFrameworkIntegrationTests
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
                "Some title",
                DateTimeOffset.Now,
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
            var existingShowtime = SeedData.ExampleShowtimeReadModel;

            var repository = this.GetReadModelRepository<ShowtimeReadModel>();

            // Act
            Func<Task> addAsync = async () => await repository.AddAsync(existingShowtime);

            // Assert
            await addAsync.Should().ThrowAsync<DbUpdateException>();
        }
    }
}