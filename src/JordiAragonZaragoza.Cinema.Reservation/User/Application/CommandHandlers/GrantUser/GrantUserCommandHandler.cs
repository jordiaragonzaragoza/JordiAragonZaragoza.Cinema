namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.GrantUser
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Cinema.Domain;

    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain;

    using JordiAragonZaragoza.Cinema.Reservation.Tenant.Domain;

    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class GrantUserCommandHandler : ICommandHandler<GrantUserCommand>
    {
        private readonly IRepository<User, UserId> userRepository;

        public GrantUserCommandHandler(IRepository<User, UserId> userRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<Result> Handle(GrantUserCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var existingUser = await this.userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken);
            if (existingUser is null)
            {
                return Result.NotFound($"{nameof(User)}: {request.UserId} not found.");
            }

            var scope = Scope.Create(
                new TenantId(request.TenantId),
                request.PartitionId.HasValue ? new PartitionId(request.PartitionId.Value) : null,
                request.CinemaId.HasValue ? new CinemaId(request.CinemaId.Value) : null);

            var roles = request.Roles.Select(Role.Create).ToList();

            existingUser.GrantUser(scope, roles);

            await this.userRepository.UpdateAsync(existingUser, cancellationToken);

            return Result.NoContent();
        }
    }
}