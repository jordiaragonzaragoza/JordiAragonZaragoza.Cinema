namespace JordiAragon.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Repositories.BusinessModel.Showtime
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinema.Reservation.IntegrationTests.Infrastructure.EntityFramework.Common;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.Cinema.Reservation.Showtime.Domain;
    using JordiAragon.Cinema.Reservation.User.Domain;
    using Microsoft.EntityFrameworkCore;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class UpdateTests : BaseEntityFrameworkIntegrationTests
    {
        public UpdateTests(
            IntegrationTestsFixture fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task UpdateAsync_WhenHavingAnExistingShowtime_ShouldUpdateTheShowtime()
        {
            // Arrange
            var repository = this.GetBusinessModelRepository<Showtime, ShowtimeId>();

            var existingShowtime = await repository.GetByIdAsync(ShowtimeId.Create(SeedData.ExampleShowtime.Id));

            var ticketId = TicketId.Create(Guid.NewGuid());

            var userId = UserId.Create(Guid.NewGuid());

            var seatIds = new List<SeatId>
            {
                SeatId.Create(Guid.NewGuid()),
            };

            var createdTimeOnUtc = DateTimeOffset.UtcNow;

            var ticket = existingShowtime.ReserveSeats(ticketId, userId, seatIds, createdTimeOnUtc);

            // Act
            await repository.UpdateAsync(existingShowtime);

            // Assert
            var result = await repository.GetByIdAsync(ShowtimeId.Create(existingShowtime.Id));

            result.Should().NotBeNull();
            result.Tickets.Should().Contain(ticket);
        }

        [Fact]
        public async Task UpdateAsync_WhenHavingAnUnexistingShowtime_ShouldThrowDbUpdateException()
        {
            // Arrange
            var newShowtime = Showtime.Schedule(
                ShowtimeId.Create(Guid.NewGuid()),
                MovieId.Create(SeedData.ExampleMovie.Id),
                DateTimeOffset.UtcNow.AddDays(1),
                AuditoriumId.Create(SeedData.ExampleAuditorium.Id));

            var repository = this.GetBusinessModelRepository<Showtime, ShowtimeId>();

            // Act
            Func<Task> deleteAsync = async () => await repository.UpdateAsync(newShowtime);

            // Assert
            await deleteAsync.Should().ThrowAsync<DbUpdateConcurrencyException>();
        }
    }
}