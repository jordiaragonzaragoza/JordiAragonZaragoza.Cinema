﻿namespace JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.V2.Showtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ardalis.HttpClientTestExtensions;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation;
    using JordiAragonZaragoza.Cinema.Reservation.FunctionalTests.Presentation.HttpRestfulApi.Common;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Auditorium.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Requests;
    using JordiAragonZaragoza.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2.Showtime.Responses;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Presentation.HttpRestfulApi.V2;
    using Xunit;
    using Xunit.Abstractions;

    using Constants = JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain.Constants;

    public sealed class ReserveSeatsTests : BaseHttpRestfulApiFunctionalTests
    {
        public ReserveSeatsTests(
            FunctionalTestsFixture<Program> fixture,
            ITestOutputHelper outputHelper)
            : base(fixture, outputHelper)
        {
        }

        [Fact]
        public async Task CreateReservation_WhenHavingValidArguments_ShouldCreateRequiredReservation()
        {
            // Arrange
            var sessionDateOnUtc = DateTimeOffset.UtcNow.AddDays(1);

            var showtimeId = await this.ScheduleNewShowtimeAsync(sessionDateOnUtc);

            var routeAvailableSeats = $"api/v2/{GetAvailableSeats.Route}";
            routeAvailableSeats = routeAvailableSeats.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);

            var availableSeatsResponse = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(routeAvailableSeats, this.OutputHelper);

            var seatsIds = availableSeatsResponse.OrderBy(s => s.Row).ThenBy(s => s.SeatNumber)
                                                 .Take(3).Select(seat => seat.Id);

            var reservationId = Guid.NewGuid();
            var reserveSeatsRequest = new ReserveSeatsRequest(reservationId, showtimeId, seatsIds);
            var reserveSeatsContent = StringContentHelpers.FromModelAsJson(reserveSeatsRequest);

            var reserveSeatsRoute = $"api/v2/{ReserveSeats.Route}";
            reserveSeatsRoute = reserveSeatsRoute.Replace("{showtimeId}", showtimeId.ToString(), StringComparison.Ordinal);
            reserveSeatsRoute = reserveSeatsRoute.Replace("{reservationId}", reservationId.ToString(), StringComparison.Ordinal);

            // Act
            var reservationResponse = await this.Fixture.HttpClient.PutAndDeserializeAsync<ReservationResponse>(reserveSeatsRoute, reserveSeatsContent, this.OutputHelper);

            await AddEventualConsistencyDelayAsync();

            var availableSeatsAfterReservation = await this.Fixture.HttpClient.GetAndDeserializeAsync<IEnumerable<SeatResponse>>(routeAvailableSeats, this.OutputHelper);

            // Assert
            reservationResponse.SessionDateOnUtc.Should()
                .Be(sessionDateOnUtc);

            reservationResponse.AuditoriumName.Should()
                .Be(SeedData.ExampleAuditorium.Name);

            reservationResponse.MovieTitle.Should()
                .Be(SeedData.ExampleMovie.Title);

            reservationResponse.Seats.Select(seatResponse => seatResponse.Id).Should()
                .Contain(seatsIds);

            reservationResponse.IsPurchased.Should().BeFalse();

            availableSeatsAfterReservation.Should().NotContain(reservationResponse.Seats);
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