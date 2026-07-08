namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Application.CommandHandlers.RemovePartition
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.Commands;

    public sealed class RemovePartitionCommandValidator : AbstractValidator<RemovePartitionCommand>
    {
        public RemovePartitionCommandValidator()
        {
            this.RuleFor(x => x.PartitionId)
              .NotEmpty().WithMessage("PartitionId is required.");
        }
    }
}