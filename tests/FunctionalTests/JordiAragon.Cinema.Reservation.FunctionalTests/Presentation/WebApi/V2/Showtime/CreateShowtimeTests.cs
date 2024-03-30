namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.WebApi.V2.Showtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.WebApi.Common;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Auditorium.Responses;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Presentation.WebApi.V2;
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
            // Arrange Showtime Creation
            var url = $"api/v2/{CreateShowtime.Route}";

            var sessionDateOnUtc = DateTimeOffset.UtcNow.AddDays(1);

            var request = new CreateShowtimeRequest(
                SeedData.ExampleAuditorium.Id,
                SeedData.ExampleMovie.Id,
                sessionDateOnUtc);

            var content = StringContentHelpers.FromModelAsJson(request);

            // Act Showtime Creation
            var showtimeId = await this.Fixture.HttpClient.PostAndDeserializeAsync<Guid>(url, content, this.OutputHelper);

            // Assert Showtime Creation
            showtimeId.Should()
                .NotBeEmpty();

            await Task.Delay(TimeSpan.FromSeconds(2)); // Required to satisfy eventual consistency on projections.

            // Arrange Showtime Projection
            var getShowtimeRoute = $"api/v2/{GetShowtime.Route}";
            string pathAndQuery = EndpointRouteHelpers.BuildUriWithQueryParameters(
                getShowtimeRoute,
                (nameof(showtimeId), showtimeId.ToString()));

            // Act Showtime Projection
            var showtimeResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<ShowtimeResponse>(pathAndQuery, this.OutputHelper);

            // Assert Showtime Projection
            showtimeResponse.Should().NotBeNull();
            showtimeResponse.MovieTitle.Should().Be(SeedData.ExampleMovie.Title);
            showtimeResponse.SessionDateOnUtc.Should().Be(sessionDateOnUtc);
            showtimeResponse.AuditoriumId.Should().Be(SeedData.ExampleAuditorium.Id);
            showtimeResponse.AuditoriumName.Should().Be(SeedData.ExampleAuditorium.Name);

            // Arrange AvailableSeats Projection
            var route = $"api/v2/{GetAvailableSeats.Route}";
            route = route.Replace("{showtimeId}", showtimeId.ToString());

            // Act AvailableSeats Projection
            var availableSeatsResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(route, this.OutputHelper);

            // Assert AvailableSeats Projection
            availableSeatsResponse.Should().NotBeNullOrEmpty();
            availableSeatsResponse.Count().Should().Be(SeedData.ExampleAuditorium.Rows * SeedData.ExampleAuditorium.SeatsPerRow);
        }
    }
}