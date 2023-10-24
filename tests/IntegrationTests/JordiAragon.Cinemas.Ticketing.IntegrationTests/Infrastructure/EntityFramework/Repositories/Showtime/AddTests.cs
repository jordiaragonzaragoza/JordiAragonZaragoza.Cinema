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

    public class AddTests : BaseEntityFrameworkIntegrationTests<Showtime>
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
            var newShowtime = Showtime.Create(
                ShowtimeId.Create(Guid.NewGuid()),
                MovieId.Create(SeedData.ExampleMovie.Id.Value),
                DateTime.UtcNow.AddDays(1),
                AuditoriumId.Create(SeedData.ExampleAuditorium.Id));

            var repository = this.GetRepository();

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
            var existingShowtime = SeedData.ExampleShowtime;

            var repository = this.GetRepository();

            // Act
            Func<Task> addAsync = async () => await repository.AddAsync(existingShowtime);

            // Assert
            await addAsync.Should().ThrowAsync<DbUpdateException>();
        }
    }
}