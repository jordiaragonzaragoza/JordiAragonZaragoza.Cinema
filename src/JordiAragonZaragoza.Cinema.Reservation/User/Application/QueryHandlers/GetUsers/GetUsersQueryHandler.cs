﻿namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUsers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using AutoMapper;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    // TODO: Temporal. Move. This query is part of other bounded context. (Management Bounded Context)
    public sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, IEnumerable<UserOutputDto>>
    {
        private readonly IReadListRepository<User, UserId> userRepository;
        private readonly IMapper mapper;

        public GetUsersQueryHandler(
            IReadListRepository<User, UserId> userRepository,
            IMapper mapper)
        {
            this.userRepository = Guard.Against.Null(userRepository, nameof(userRepository));
            this.mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public async Task<Result<IEnumerable<UserOutputDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await this.userRepository.ListAsync(cancellationToken);
            if (users.Count == 0)
            {
                return Result.NotFound($"{nameof(User)}/s not found.");
            }

            return Result.Success(this.mapper.Map<IEnumerable<UserOutputDto>>(users));
        }
    }
}