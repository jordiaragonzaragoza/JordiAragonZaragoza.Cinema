namespace JordiAragon.Cinema.Reservation.Auditorium.Application.CommandHanders.RemoveAuditorium
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragon.Cinema.Reservation.Auditorium.Application.Contracts.Commands;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;
    using JordiAragon.SharedKernel.Application.Commands;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public sealed class RemoveAuditoriumCommandHandler : BaseCommandHandler<RemoveAuditoriumCommand>
    {
        private readonly IRepository<Auditorium, AuditoriumId> auditoriumRepository;

        public RemoveAuditoriumCommandHandler(IRepository<Auditorium, AuditoriumId> auditoriumRepository)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
        }

        public override async Task<Result> Handle(RemoveAuditoriumCommand command, CancellationToken cancellationToken)
        {
            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(AuditoriumId.Create(command.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {command.AuditoriumId} not found.");
            }

            // TODO: Sagas. Before remove auditorium check if there is some scheduled showtime regarding to auditorium.
            existingAuditorium.Remove();

            await this.auditoriumRepository.DeleteAsync(existingAuditorium, cancellationToken);

            return Result.Success();
        }
    }
}