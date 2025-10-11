namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.QueryHandlers.GetAuditoriums
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed class GetAuditoriumsSpec : Specification<AuditoriumReadModel>, IPaginatedSpecification<AuditoriumReadModel>
    {
        private readonly GetAuditoriumsQuery request;

        public GetAuditoriumsSpec(GetAuditoriumsQuery request)
        {
            this.request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public IPaginatedQuery Request
            => this.request;
    }
}