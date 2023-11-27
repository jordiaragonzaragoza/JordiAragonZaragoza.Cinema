﻿namespace JordiAragon.Cinema.Reservation.Auditorium.Application.Queries.GetAuditoriums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Queries;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragon.SharedKernel.Domain.Contracts.Interfaces;

    public class GetAuditoriumsQueryHandler : IQueryHandler<GetAuditoriumsQuery, IEnumerable<AuditoriumOutputDto>>
    {
        private readonly IReadRepository<Auditorium, AuditoriumId, Guid> auditoriumRepository;
        private readonly IMapper mapper;

        public GetAuditoriumsQueryHandler(
            IReadRepository<Auditorium, AuditoriumId, Guid> auditoriumRepository,
            IMapper mapper)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public async Task<Result<IEnumerable<AuditoriumOutputDto>>> Handle(GetAuditoriumsQuery request, CancellationToken cancellationToken)
        {
            var auditoriums = await this.auditoriumRepository.ListAsync(cancellationToken);
            if (!auditoriums.Any())
            {
                return Result.NotFound($"{nameof(Auditorium)}/s not found.");
            }

            return Result.Success(this.mapper.Map<IEnumerable<AuditoriumOutputDto>>(auditoriums));
        }
    }
}