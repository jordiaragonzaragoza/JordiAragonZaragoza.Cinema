namespace JordiAragon.Cinema.Reservation.Auditorium.Application.CommandHanders.CreateAuditorium
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public sealed class CreateAuditoriumCommandHandler : BaseCommandHandler<CreateAuditoriumCommand>
    {
        private readonly IRepository<Auditorium, AuditoriumId> auditoriumRepository;

        public CreateAuditoriumCommandHandler(IRepository<Auditorium, AuditoriumId> auditoriumRepository)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
        }

        public override async Task<Result> Handle(CreateAuditoriumCommand command, CancellationToken cancellationToken)
        {
            // TODO: There cannot be two Auditoriums with the same name.
            var newAuditorium = Auditorium.Create(
                id: AuditoriumId.Create(command.AuditoriumId),
                name: command.Name,
                rows: command.Rows,
                seatsPerRow: command.SeatsPerRow);

            await this.auditoriumRepository.AddAsync(newAuditorium, cancellationToken);

            return Result.Success();
        }
    }
}