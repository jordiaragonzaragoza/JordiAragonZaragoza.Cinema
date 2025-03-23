namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.V2.Showtime
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2;
    using Xunit;
    using Xunit.Abstractions;

    using Constants = JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain.Constants;

    public sealed class GetShowtimeTests : BaseHttpRestfulApiFunctionalTests
    {
        public GetShowtimeTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetShowtime_WhenHavingValidArguments_ShouldReturnOneShowtime()
        {
            // Arrange
            var sessionDateOnUtc = DateTimeOffset.UtcNow.AddDays(1);

            var showtimeId = await this.ScheduleNewShowtimeAsync(sessionDateOnUtc);

            var route = $"api/v2/{GetShowtime.Route}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(showtimeId), showtimeId.ToString()));

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<ShowtimeResponse>(uri.PathAndQuery, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
            response.MovieTitle.Should().Be(SeedData.ExampleMovie.Title);
            response.SessionDateOnUtc.Should().Be(sessionDateOnUtc);
            response.AuditoriumId.Should().Be(SeedData.ExampleAuditorium.Id);
            response.AuditoriumName.Should().Be(SeedData.ExampleAuditorium.Name);
        }

        private async Task<Guid> ScheduleNewShowtimeAsync(DateTimeOffset sessionDateOnUtc)
        {
            var showtimeId = Guid.NewGuid();

            var route = $"api/v2/{ScheduleShowtime.Route}";
            route = route.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            var request = new ScheduleShowtimeRequest(
                showtimeId,
                Constants.Auditorium.Id,
                Constants.Movie.Id,
                sessionDateOnUtc);

            var content = StringContentHelpers.FromModelAsJson(request);

            var fullUri = new Uri(this.Fixture.HttpClient.BaseAddress!, route);

            this.OutputHelper.WriteLine($"Requesting with PUT {route}");
            await this.Fixture.HttpClient.PutAsync(fullUri, content);

            await AddEventualConsistencyDelayAsync();

            return showtimeId;
        }
    }
}