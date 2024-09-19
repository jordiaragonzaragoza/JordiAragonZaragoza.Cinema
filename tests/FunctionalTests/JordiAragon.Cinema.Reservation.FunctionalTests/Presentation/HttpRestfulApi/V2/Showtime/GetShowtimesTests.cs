namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.V2.Showtime
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2;
    using JordiAragon.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using Xunit;
    using Xunit.Abstractions;

    using Constants = JordiAragon.Cinema.Reservation.TestUtilities.Domain.Constants;

    public sealed class GetShowtimesTests : BaseHttpRestfulApiFunctionalTests
    {
        public GetShowtimesTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetAllShowtimes_WhenHavingValidArguments_ShouldReturnOneShowtime()
        {
            // Arrange
            var sessionDateOnUtc = DateTimeOffset.UtcNow.AddDays(1);

            await this.ScheduleNewShowtimeAsync(sessionDateOnUtc);

            var movieId = SeedData.ExampleMovie.Id.ToString()!;
            var auditoriumId = SeedData.ExampleAuditorium.Id.ToString()!;
            var startTimeOnUtc = sessionDateOnUtc.ToString("O");
            var endTimeOnUtc = DateTimeOffset.UtcNow.AddYears(2).ToString("O");

            var route = $"api/v2/{GetShowtimes.Route}";
            var uri = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(movieId), movieId),
                (nameof(auditoriumId), auditoriumId),
                (nameof(startTimeOnUtc), startTimeOnUtc),
                (nameof(endTimeOnUtc), endTimeOnUtc));

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<PaginatedCollectionResponse<ShowtimeResponse>>(uri.PathAndQuery, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
            response.Items.Should().HaveCount(1);
        }

        private async Task<Guid> ScheduleNewShowtimeAsync(DateTimeOffset sessionDateOnUtc)
        {
            var url = $"api/v2/{ScheduleShowtime.Route}";

            var request = new ScheduleShowtimeRequest(
                Constants.Auditorium.Id,
                Constants.Movie.Id,
                sessionDateOnUtc);

            var content = StringContentHelpers.FromModelAsJson(request);

            var response = await this.Fixture.HttpClient.PostAndDeserializeAsync<Guid>(url, content, this.OutputHelper);

            await AddEventualConsistencyDelayAsync();

            return response;
        }
    }
}