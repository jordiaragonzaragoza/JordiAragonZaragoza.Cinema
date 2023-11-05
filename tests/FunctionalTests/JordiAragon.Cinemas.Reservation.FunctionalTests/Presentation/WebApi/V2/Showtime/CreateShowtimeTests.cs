namespace JordiAragon.Cinemas.Reservation.FunctionalTests.Presentation.WebApi.V2.Showtime
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinemas.Reservation;
    using JordiAragon.Cinemas.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinemas.Reservation.FunctionalTests.Presentation.WebApi.Common;
    using JordiAragon.Cinemas.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinemas.Reservation.Showtime.Presentation.WebApi.V2;
    using Xunit;
    using Xunit.Abstractions;

    public class CreateShowtimeTests : BaseWebApiFunctionalTests
    {
        public CreateShowtimeTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task CreateShowtime_WhenHavingValidArguments_ShouldCreateRequiredShowtime()
        {
            // Arrange
            var url = $"api/v2/{CreateShowtime.Route}";

            var request = new CreateShowtimeRequest(
                SeedData.ExampleAuditorium.Id,
                SeedData.ExampleMovie.Id,
                DateTime.UtcNow.AddDays(1));

            var content = StringContentHelpers.FromModelAsJson(request);

            // Act
            var response = await this.Fixture.HttpClient.PostAndDeserializeAsync<Guid>(url, content, this.OutputHelper);

            // Assert
            response.Should()
                .NotBeEmpty();
        }
    }
}