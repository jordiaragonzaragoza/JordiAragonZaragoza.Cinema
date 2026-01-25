namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUsers
{
    using System;
    using Ardalis.Specification;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed class GetUsersSpec : Specification<UserReadModel>, IPaginatedSpecification<UserReadModel>
    {
        private readonly GetUsersQuery request;

        public GetUsersSpec(GetUsersQuery request)
        {
            this.request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public IPaginatedQuery Request
            => this.request;
    }
}