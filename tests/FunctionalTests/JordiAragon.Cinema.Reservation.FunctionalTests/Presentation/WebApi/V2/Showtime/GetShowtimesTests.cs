namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.WebApi.V2.Showtime
{
    using System;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Configuration;
    using JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.WebApi.Common;
    using JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Reservation.Showtime.Presentation.WebApi.V2;
    using JordiAragon.SharedKernel.Presentation.WebApi.Contracts;
    using Xunit;
    using Xunit.Abstractions;

    public class GetShowtimesTests : BaseWebApiFunctionalTests
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
            var movieId = SeedData.ExampleMovie.Id.ToString();
            var auditoriumId = SeedData.ExampleAuditorium.Id.ToString();
            var startTimeOnUtc = SeedData.ExampleShowtime.SessionDateOnUtc.ToString("O");
            var endTimeOnUtc = DateTime.UtcNow.AddYears(2).ToString("O");

            var route = $"api/v2/{GetShowtimes.Route}";
            string pathAndQuery = EndpointRouteHelpers.BuildUriWithQueryParameters(
                route,
                (nameof(movieId), movieId),
                (nameof(auditoriumId), auditoriumId),
                (nameof(startTimeOnUtc), startTimeOnUtc),
                (nameof(endTimeOnUtc), endTimeOnUtc));

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<PaginatedCollectionResponse<ShowtimeResponse>>(pathAndQuery, this.OutputHelper);

            // Assert
            response.Should().NotBeNull();
            response.Items.Should().HaveCount(1);
        }
    }
}