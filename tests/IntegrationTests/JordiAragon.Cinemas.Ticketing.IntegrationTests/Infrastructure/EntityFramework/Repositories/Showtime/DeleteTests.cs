namespace JordiAragon.Cinemas.Ticketing.IntegrationTests.Infrastructure.EntityFramework.Repositories.Showtime
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Auditorium.Domain;
    using JordiAragon.Cinemas.Ticketing.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinemas.Ticketing.IntegrationTests.Infrastructure.EntityFramework.Common;
    using JordiAragon.Cinemas.Ticketing.Movie.Domain;
    using JordiAragon.Cinemas.Ticketing.Showtime.Domain;
    using Microsoft.EntityFrameworkCore;
    using Xunit;
    using Xunit.Abstractions;

    public class DeleteTests : BaseEntityFrameworkIntegrationTests<Showtime>
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
            var existingShowtime = SeedData.ExampleShowtime;

            var repository = this.GetRepository();

            // Act
            await repository.DeleteAsync(existingShowtime);

            // Assert
            var result = await repository.ListAsync();

            result.Should()
                .NotContain(existingShowtime);
        }

        [Fact]
        public async Task DeleteAsync_WhenHavingAnUnexistingShowtime_ShouldThrowDbUpdateException()
        {
            // Arrange
            var newShowtime = Showtime.Create(
                ShowtimeId.Create(Guid.NewGuid()),
                MovieId.Create(SeedData.ExampleMovie.Id.Value),
                DateTime.UtcNow.AddDays(1),
                AuditoriumId.Create(SeedData.ExampleAuditorium.Id));

            var repository = this.GetRepository();

            // Act
            Func<Task> deleteAsync = async () => await repository.DeleteAsync(newShowtime);

            // Assert
            await deleteAsync.Should().ThrowAsync<DbUpdateConcurrencyException>();
        }
    }
}