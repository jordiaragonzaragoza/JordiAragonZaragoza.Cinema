namespace JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Queries
{
    using System.Collections.Generic;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class GetAuditoriumsQuery : IQuery<IEnumerable<AuditoriumOutputDto>>;
}