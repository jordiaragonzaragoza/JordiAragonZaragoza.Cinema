namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries
{
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class GetUsersQuery : IQuery<IEnumerable<UserOutputDto>>;
}