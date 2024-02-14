namespace JordiAragon.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Repositories.ReadModel.Showtime
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common;
    using JordiAragon.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using Microsoft.EntityFrameworkCore;
    using Xunit;
    using Xunit.Abstractions;

    public class DeleteTests : BaseEntityFrameworkIntegrationTests
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
            var newShowtime = new ShowtimeReadModel(
                Guid.NewGuid(),
                DateTimeOffset.Now,
                Guid.NewGuid(),
                "Some title",
                TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28),
                Guid.NewGuid(),
                "Some auditorium");

            var repository = this.GetReadModelRepository<ShowtimeReadModel>();

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
            var newShowtime = new ShowtimeReadModel(
                Guid.NewGuid(),
                DateTimeOffset.Now,
                Guid.NewGuid(),
                "Some title",
                TimeSpan.FromHours(2) + TimeSpan.FromMinutes(28),
                Guid.NewGuid(),
                "Some auditorium");

            var repository = this.GetReadModelRepository<ShowtimeReadModel>();

            // Act
            Func<Task> deleteAsync = async () => await repository.DeleteAsync(newShowtime);

            // Assert
            await deleteAsync.Should().ThrowAsync<DbUpdateConcurrencyException>();
        }
    }
}