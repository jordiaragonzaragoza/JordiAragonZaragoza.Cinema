namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.QueryHandlers.GetUsers
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Queries;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;

    // TODO: Temporal. Move. This query is part of other bounded context(Catalog)
    public sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, PaginatedCollectionOutputDto<UserReadModel>>
    {
        private readonly IPaginatedSpecificationReadRepository<UserReadModel> auditoriumReadModelRepository;

        public GetUsersQueryHandler(IPaginatedSpecificationReadRepository<UserReadModel> auditoriumReadModelRepository)
        {
            this.auditoriumReadModelRepository = auditoriumReadModelRepository ?? throw new ArgumentNullException(nameof(auditoriumReadModelRepository));
        }

        public async Task<Result<PaginatedCollectionOutputDto<UserReadModel>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetUsersSpec(request);
            var result = await this.auditoriumReadModelRepository.PaginatedListAsync(specification, cancellationToken);
            if (!result.Items.Any())
            {
                return Result.NotFound("User/s not found.");
            }

            return Result.Success(result);
        }
    }
}