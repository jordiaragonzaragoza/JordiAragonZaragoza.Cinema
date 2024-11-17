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
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
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
            var repository = this.GetBusinessModelRepository<Showtime, ShowtimeId>();

            var newShowtime = await AddNewShowtimeAsync(repository);

            // Act
            var result = await repository.GetByIdAsync(new ShowtimeId(newShowtime.Id));

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
            var result = await repository.GetByIdAsync(new ShowtimeId(Guid.NewGuid()));

            // Assert
            result.Should()
                .BeNull();
        }

        private static async Task<Showtime> AddNewShowtimeAsync(ReservationRepository<Showtime, ShowtimeId> repository)
        {
            var newShowtime = Showtime.Schedule(
                new ShowtimeId(Guid.NewGuid()),
                new MovieId(Constants.Movie.Id),
                DateTimeOffset.UtcNow.AddDays(1),
                new AuditoriumId(Constants.Auditorium.Id));

            await repository.AddAsync(newShowtime);

            return newShowtime;
        }
    }
}