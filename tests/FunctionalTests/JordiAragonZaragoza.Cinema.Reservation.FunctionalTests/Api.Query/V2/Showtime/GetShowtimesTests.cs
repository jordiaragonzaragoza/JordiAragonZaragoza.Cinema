namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Query.V2.Showtime
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Query.Common;
    using JordiAragonZaragoza.SharedKernel.Presentation.HttpRestfulApi.Contracts;
    using Xunit;
    using Xunit.Abstractions;

    using SeedData = JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.SeedData;

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

            var movieId = SeedData.ExampleMovie.Id.ToString()!;
            var auditoriumId = SeedData.ExampleAuditorium.Id.ToString()!;
            var startTimeOnUtc = sessionDateOnUtc.ToString("O");
            var endTimeOnUtc = DateTimeOffset.UtcNow.AddYears(2).ToString("O");

            var route = $"{Routes.ApiBase}{ShowtimeRoutes.GetShowtimes}";
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
    }
}