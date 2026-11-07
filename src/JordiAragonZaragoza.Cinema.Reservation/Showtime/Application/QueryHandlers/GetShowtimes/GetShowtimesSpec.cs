namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.QueryHandlers.GetShowtimes
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed class GetShowtimesSpec : Specification<ShowtimeReadModel>, IPaginatedSpecification<ShowtimeReadModel>
    {
        private readonly GetShowtimesQuery request;

        public GetShowtimesSpec(GetShowtimesQuery request)
        {
            this.request = request ?? throw new ArgumentNullException(nameof(request));

            this.Query
                .Where(s => s.AuditoriumId == request.AuditoriumId, request.AuditoriumId is not null)
                .Where(s => s.MovieId == request.MovieId, request.MovieId is not null)
                .Where(s => s.AuditoriumName.Contains(request.AuditoriumName!), !string.IsNullOrWhiteSpace(request.AuditoriumName))
                .Where(s => s.MovieTitle.Contains(request.MovieTitle!), !string.IsNullOrWhiteSpace(request.MovieTitle))
                .Where(s => s.SessionDateOnUtc >= request.StartTimeOnUtc, request.StartTimeOnUtc is not null)
                .Where(s => s.SessionDateOnUtc <= request.EndTimeOnUtc, request.EndTimeOnUtc is not null)
                .AsNoTracking()
                .OrderByDescending(x => x.SessionDateOnUtc);
        }

        public IPaginatedQuery Request
            => this.request;
    }
}