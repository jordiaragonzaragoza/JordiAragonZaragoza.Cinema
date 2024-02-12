namespace JordiAragon.Cinema.Reservation.Presentation.WebApi.Contracts.V2.Showtime.Requests
{
    using System;
    using JordiAragon.SharedKernel.Presentation.WebApi.Contracts;

    public record class GetShowtimesRequest(Guid? AuditoriumId, Guid? MovieId, DateTime? StartTimeOnUtc, DateTime? EndTimeOnUtc, int PageNumber = 1, int PageSize = 10) : IPaginatedRequest;
}