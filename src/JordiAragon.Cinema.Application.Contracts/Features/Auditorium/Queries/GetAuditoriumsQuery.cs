namespace JordiAragon.Cinema.Application.Contracts.Features.Auditorium.Queries
{
    using System.Collections.Generic;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;

    public record class GetAuditoriumsQuery : IQuery<IEnumerable<AuditoriumOutputDto>>;
}