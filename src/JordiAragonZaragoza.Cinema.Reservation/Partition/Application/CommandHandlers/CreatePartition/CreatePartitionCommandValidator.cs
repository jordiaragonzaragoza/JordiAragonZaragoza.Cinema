namespace JordiAragonZaragoza.Cinema.Reservation.Partition.Application.CommandHandlers.AddPartition
{
    using FluentValidation;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Application.Contracts.Commands;

    public sealed class CreatePartitionCommandValidator : AbstractValidator<CreatePartitionCommand>
    {
        public CreatePartitionCommandValidator()
        {
            this.RuleFor(x => x.PartitionId)
              .NotEmpty().WithMessage("PartitionId is required.");
        }
    }
}