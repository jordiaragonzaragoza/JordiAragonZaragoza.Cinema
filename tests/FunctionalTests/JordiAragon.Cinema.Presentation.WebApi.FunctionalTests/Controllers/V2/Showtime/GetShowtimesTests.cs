namespace JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Controllers.V2.Showtime
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Infrastructure.EntityFramework.AssemblyConfiguration;
    using JordiAragon.Cinema.Presentation.WebApi.Contracts.V2.Showtime.Responses;
    using JordiAragon.Cinema.Presentation.WebApi.Controllers.V2;
    using JordiAragon.Cinema.Presentation.WebApi.FunctionalTests.Common;
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

        protected override string ControllerBasePath
        {
            get => ControllerRouteHelpers.GetControllerBasePath<ShowtimesController>();
        }

        public static IEnumerable<object[]> InvalidArgumentsGetAllShowtimes()
        {
            var movieId = SeedData.ExampleMovie.Id.ToString();
            var auditoriumId = SeedData.ExampleAuditorium.Id.ToString();
            var startTimeOnUtc = SeedData.ExampleShowtime.SessionDateOnUtc.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var endTimeOnUtc = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            var movieIdValues = new object[] { null, string.Empty, " ", movieId };
            var auditoriumIdValues = new object[] { null, string.Empty, " ", auditoriumId };
            var startTimeOnUtcValues = new object[] { null, string.Empty, " ", SeedData.ExampleShowtime.SessionDateOnUtc.ToString(), startTimeOnUtc };
            var endTimeOnUtcValues = new object[] { null, string.Empty, " ", DateTime.UtcNow.ToString(), endTimeOnUtc };

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

        [Fact]
        public async Task GetAllShowtimes_WhenHavingValidArguments_ShouldReturnOneShowtime()
        {
            // Arrange
            var movieId = SeedData.ExampleMovie.Id.ToString();
            var auditoriumId = SeedData.ExampleAuditorium.Id.ToString();

            string pathAndQuery = ControllerRouteHelpers.BuildUriWithQueryParameters(
                this.ControllerBasePath,
                (nameof(movieId), movieId),
                (nameof(auditoriumId), auditoriumId));

            // Act
            var response = await this.HttpClient.GetAndDeserializeAsync<IEnumerable<ShowtimeResponse>>(pathAndQuery, this.OutputHelper);

            // Assert
            response.Should()
                .NotBeNullOrEmpty()
                .And
            .HaveCount(1);
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentsGetAllShowtimes))]
        public async Task XXX_GetAllShowtimes_WhenHavingInValidArguments_ShouldReturnBadRequest(
            string movieId,
            string auditoriumId,
            string startTimeOnUtc,
            string endTimeOnUtc)
        {
            string pathAndQuery = ControllerRouteHelpers.BuildUriWithQueryParameters(
                this.ControllerBasePath,
                (nameof(movieId), movieId),
                (nameof(auditoriumId), auditoriumId),
                (nameof(startTimeOnUtc), startTimeOnUtc),
                (nameof(endTimeOnUtc), endTimeOnUtc));

            // Act
            var response = await this.HttpClient.GetAndEnsureBadRequestAsync(pathAndQuery, this.OutputHelper);

            // Assert
            response.StatusCode.Should()
                .Be(HttpStatusCode.BadRequest);
        }
    }
}