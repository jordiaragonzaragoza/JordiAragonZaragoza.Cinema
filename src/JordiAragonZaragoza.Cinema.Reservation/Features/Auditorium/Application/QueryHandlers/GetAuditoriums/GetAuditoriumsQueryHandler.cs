namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.QueryHandlers.GetAuditoriums
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    // TODO: Temporal. Move. This query is part of other bounded context. (Management Bounded Context)
    public sealed class GetAuditoriumsQueryHandler : IQueryHandler<GetAuditoriumsQuery, IEnumerable<AuditoriumOutputDto>>
    {
        private readonly IReadListRepository<Auditorium, AuditoriumId> auditoriumRepository;
        private readonly IMapper mapper;

        public GetAuditoriumsQueryHandler(
            IReadListRepository<Auditorium, AuditoriumId> auditoriumRepository,
            IMapper mapper)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public async Task<Result<IEnumerable<AuditoriumOutputDto>>> Handle(GetAuditoriumsQuery request, CancellationToken cancellationToken)
        {
            var auditoriums = await this.auditoriumRepository.ListAsync(cancellationToken);
            if (auditoriums.Count == 0)
            {
                return Result.NotFound($"{nameof(Auditorium)}/s not found.");
            }

            return Result.Success(this.mapper.Map<IEnumerable<AuditoriumOutputDto>>(auditoriums));
        }
    }
}