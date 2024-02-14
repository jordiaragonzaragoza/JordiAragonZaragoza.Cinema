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

        /*
        public static IEnumerable<object[]> InvalidArgumentsGetAllShowtimes()
        {
            var movieId = SeedData.ExampleMovie.Id.ToString();
            var auditoriumId = SeedData.ExampleAuditorium.Id.ToString();
            var startTimeOnUtc = SeedData.ExampleShowtime.SessionDateOnUtc.ToString("O");
            var endTimeOnUtc = DateTime.UtcNow.ToString("O");

            var movieIdValues = new object[] { null, string.Empty, " ", movieId };
            var auditoriumIdValues = new object[] { null, string.Empty, " ", auditoriumId };
            var startTimeOnUtcValues = new object[] { null, string.Empty, " ", default(DateTime).ToString("O"), startTimeOnUtc };
            var endTimeOnUtcValues = new object[] { null, string.Empty, " ", default(DateTime).ToString("O"), endTimeOnUtc };

            foreach (var movieIdValue in movieIdValues)
            {
                foreach (var auditoriumIdValue in auditoriumIdValues)
                {
                    foreach (var startTimeOnUtcValue in startTimeOnUtcValues)
                    {
                        foreach (var endTimeOnUtcValue in endTimeOnUtcValues)
                        {
                            if (auditoriumIdValue != null && auditoriumIdValue.Equals(auditoriumId))
                            {
                                continue;
                            }

                            yield return new object[] { movieIdValue, auditoriumIdValue, startTimeOnUtcValue, endTimeOnUtcValue };
                        }
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsGetAllShowtimes))]
        public async Task GetAllShowtimes_WhenHavingInValidArguments_ShouldReturnBadRequest(
            string movieId,
            string auditoriumId,
            string startTimeOnUtc,
            string endTimeOnUtc)
        {
            string pathAndQuery = ControllerRouteHelpers.BuildUriWithQueryParameters(
                this.ControllerBaseRoute,
                (nameof(movieId), movieId),
                (nameof(auditoriumId), auditoriumId),
                (nameof(startTimeOnUtc), startTimeOnUtc),
                (nameof(endTimeOnUtc), endTimeOnUtc));

            // Act
            var response = await this.Fixture.HttpClient.GetAndEnsureBadRequestAsync(pathAndQuery, this.OutputHelper);

            // Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.BadRequest);
        }*/

        [Fact]
        public async Task GetAllShowtimes_WhenHavingValidArguments_ShouldReturnOneShowtime()
        {
            // Arrange
            var movieId = SeedData.ExampleMovie.Id.ToString();
            var auditoriumId = SeedData.ExampleAuditorium.Id.ToString();
            var startTimeOnUtc = SeedData.ExampleShowtime.SessionDateOnUtc.ToString("O");
            var endTimeOnUtc = DateTime.UtcNow.ToString("O");

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