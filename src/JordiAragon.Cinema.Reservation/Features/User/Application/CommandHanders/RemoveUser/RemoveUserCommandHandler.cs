namespace JordiAragon.Cinema.Reservation.User.Application.CommandHanders.RemoveUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.User.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.User.Domain;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public sealed class RemoveUserCommandHandler : BaseCommandHandler<RemoveUserCommand>
    {
        private readonly IRepository<User, UserId> userRepository;

        public RemoveUserCommandHandler(IRepository<User, UserId> userRepository)
        {
            this.userRepository = Guard.Against.Null(userRepository, nameof(userRepository));
        }

        public override async Task<Result> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var existingUser = await this.userRepository.GetByIdAsync(UserId.Create(request.UserId), cancellationToken);
            if (existingUser is null)
            {
                return Result.NotFound($"{nameof(User)}: {request.UserId} not found.");
            }

            // TODO: Before remove user check if there is some scheduled showtime regarding to user via domain service.
            existingUser.Remove();

            await this.userRepository.DeleteAsync(existingUser, cancellationToken);

            return Result.Success();
        }
    }
}