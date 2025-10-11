namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RemoveUser
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class RemoveUserCommandHandler : ICommandHandler<RemoveUserCommand>
    {
        private readonly IRepository<User, UserId> userRepository;

        public RemoveUserCommandHandler(IRepository<User, UserId> userRepository)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<Result> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var existingUser = await this.userRepository.GetByIdAsync(new UserId(request.UserId), cancellationToken);
            if (existingUser is null)
            {
                return Result.NotFound($"{nameof(User)}: {request.UserId} not found.");
            }

            // TODO: Before remove user check if there is some scheduled showtime regarding to user via domain service.
            existingUser.Remove();

            await this.userRepository.DeleteAsync(existingUser, cancellationToken);

            return Result.NoContent();
        }
    }
}