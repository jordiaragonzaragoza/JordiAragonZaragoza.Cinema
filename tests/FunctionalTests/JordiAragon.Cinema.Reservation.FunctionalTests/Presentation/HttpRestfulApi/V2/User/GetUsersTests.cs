namespace JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.V2.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.User.Presentation.HttpRestfulApi.V2;
    using JordiAragon.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.User.Responses;
    using Xunit;
    using Xunit.Abstractions;

    public sealed class GetUsersTests : BaseHttpRestfulApiFunctionalTests
    {
        public GetUsersTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task GetAllUsers_WhenHavingValidUrl_ShouldReturnOneUser()
        {
            // Arrange
            var url = $"api/v2/{GetUsers.Route}";

            // Act
            var response = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<UserResponse>>(url, this.OutputHelper);

            // Assert
            response.Should()
                .NotBeNullOrEmpty()
                .And
                .HaveCount(1);
        }
    }
}