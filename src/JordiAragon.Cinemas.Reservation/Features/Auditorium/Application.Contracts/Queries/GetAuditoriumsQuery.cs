namespace JordiAragon.Cinemas.Reservation.Auditorium.Application.Contracts.Queries
{
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class GetAuditoriumsQuery : IQuery<IEnumerable<AuditoriumOutputDto>>;
}