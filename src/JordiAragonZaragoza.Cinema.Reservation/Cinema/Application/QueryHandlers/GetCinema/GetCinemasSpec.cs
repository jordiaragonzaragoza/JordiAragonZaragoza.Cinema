namespace JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.QueryHandlers.GetCinemas
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed class GetCinemasSpec : Specification<CinemaReadModel>, IPaginatedSpecification<CinemaReadModel>
    {
        private readonly GetCinemasQuery request;

        public GetCinemasSpec(GetCinemasQuery request)
        {
            this.request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public IPaginatedQuery Request
            => this.request;
    }
}