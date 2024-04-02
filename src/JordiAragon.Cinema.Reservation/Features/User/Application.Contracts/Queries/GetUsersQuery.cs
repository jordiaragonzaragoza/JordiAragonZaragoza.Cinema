namespace JordiAragon.Cinema.Reservation.User.Application.Contracts.Queries
{
    using System.Collections.Generic;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class GetUsersQuery : IQuery<IEnumerable<UserOutputDto>>;
}