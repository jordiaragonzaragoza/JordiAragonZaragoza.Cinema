namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.CommandHandlers.RevokeUser
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts.Commands;

    public sealed class RevokeUserCommandValidator : AbstractValidator<RevokeUserCommand>
    {
        public RevokeUserCommandValidator()
        {
            this.RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");

            this.RuleFor(x => x.TenantId)
                .NotEmpty().WithMessage("TenantId is required.");
        }
    }
}