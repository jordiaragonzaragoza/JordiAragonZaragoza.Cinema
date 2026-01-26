namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Command.V2.Showtime
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.Showtime;
    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Api.Command.Common;
    using Xunit;
    using Xunit.Abstractions;

    using SeedData = JordiAragonZaragoza.Cinema.Reservation.Worker.Seeder.SeedData;

    public sealed class PurchaseReservationTests : BaseHttpRestfulApiFunctionalTests
    {
        public PurchaseReservationTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task PurchaseExistingReservation_WhenHavingValidArguments_ShouldMarkReservationAsPaid()
        {
            // Arrange
            var showtimeId = SeedData.ExampleShowtime.Id;

            var reservationId = SeedData.ExampleReservation.Id;

            var routePurchaseReservation = $"{Routes.ApiBase}{ShowtimeRoutes.PurchaseReservation}";
            routePurchaseReservation = routePurchaseReservation.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);
            routePurchaseReservation = routePurchaseReservation.Replace("{reservationId}", reservationId.ToString(), StringComparison.Ordinal);

            var fullUriPurchaseReservation = new Uri(this.Fixture.HttpClient.BaseAddress!, routePurchaseReservation);

            // Act
            this.OutputHelper.WriteLine($"Requesting with PATCH {routePurchaseReservation}");
            var response = await this.Fixture.HttpClient.PatchAsync(fullUriPurchaseReservation, null);

            // Assert
            response.StatusCode.Should()
               .Be(HttpStatusCode.NoContent);
        }
    }
}