namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUserAuthorization
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class GetUserAuthorizationQueryHandler : IQueryHandler<GetUserAuthorizationQuery, UserAuthorizationReadModel>
    {
        private readonly ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> userAuthorizationReadModelRepository;

        public GetUserAuthorizationQueryHandler(
            ICachedSpecificationRepository<UserAuthorizationReadModel, Guid> userAuthorizationReadModelRepository)
        {
            this.userAuthorizationReadModelRepository = userAuthorizationReadModelRepository ?? throw new ArgumentNullException(nameof(userAuthorizationReadModelRepository));
        }

        public async Task<Result<UserAuthorizationReadModel>> Handle(GetUserAuthorizationQuery query, CancellationToken cancellationToken)
        {
            var specification = new GetUserAuthorizationSpecification(query);
            var result = await this.userAuthorizationReadModelRepository.FirstOrDefaultAsync(specification, cancellationToken);
            if (result is null)
            {
                return Result.NotFound($"{nameof(UserAuthorizationReadModel)} not found.");
            }

            return Result.Success(result);
        }
    }
}
