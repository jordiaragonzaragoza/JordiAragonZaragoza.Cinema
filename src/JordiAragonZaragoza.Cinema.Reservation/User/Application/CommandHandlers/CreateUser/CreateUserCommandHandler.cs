namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.CreateUser
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Commands;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class CreateUserCommandHandler : BaseCommandHandler<CreateUserCommand>
    {
        private readonly IRepository<User, UserId> userRepository;

        public CreateUserCommandHandler(IRepository<User, UserId> userRepository)
        {
            this.userRepository = Guard.Against.Null(userRepository, nameof(userRepository));
        }

        public override async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            Guard.Against.Null(request, nameof(request));

            var newUser = User.Create(
                id: UserId.Create(request.UserId));

            await this.userRepository.AddAsync(newUser, cancellationToken);

            return Result.Success();
        }
    }
}