namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.CreateUser
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IRepository<User, UserId> userRepository;

        public CreateUserCommandHandler(IRepository<User, UserId> userRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var newUser = User.Create(
                id: new UserId(request.UserId));

            await this.userRepository.AddAsync(newUser, cancellationToken);

            return Result.NoContent();
        }
    }
}