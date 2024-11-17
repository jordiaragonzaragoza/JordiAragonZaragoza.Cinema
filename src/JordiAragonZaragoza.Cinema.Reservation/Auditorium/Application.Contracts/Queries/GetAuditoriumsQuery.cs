namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Queries
{
    using System.Collections.Generic;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    public sealed record class GetAuditoriumsQuery : IQuery<IEnumerable<AuditoriumOutputDto>>;
}