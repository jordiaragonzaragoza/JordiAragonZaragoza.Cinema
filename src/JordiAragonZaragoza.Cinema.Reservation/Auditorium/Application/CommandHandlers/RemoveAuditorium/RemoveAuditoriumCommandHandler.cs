﻿namespace JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.CommandHandlers.RemoveAuditorium
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.GuardClauses;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Application.Contracts.Commands;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using JordiAragonZaragoza.SharedKernel.Application.Commands;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public sealed class RemoveAuditoriumCommandHandler : BaseCommandHandler<RemoveAuditoriumCommand>
    {
        private readonly IRepository<Auditorium, AuditoriumId> auditoriumRepository;

        public RemoveAuditoriumCommandHandler(IRepository<Auditorium, AuditoriumId> auditoriumRepository)
        {
            this.auditoriumRepository = Guard.Against.Null(auditoriumRepository, nameof(auditoriumRepository));
        }

        public override async Task<Result> Handle(RemoveAuditoriumCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            var existingAuditorium = await this.auditoriumRepository.GetByIdAsync(new AuditoriumId(request.AuditoriumId), cancellationToken);
            if (existingAuditorium is null)
            {
                return Result.NotFound($"{nameof(Auditorium)}: {request.AuditoriumId} not found.");
            }

            // TODO: Sagas. Before remove auditorium check if there is some scheduled showtime regarding to auditorium.
            existingAuditorium.Remove();

            await this.auditoriumRepository.DeleteAsync(existingAuditorium, cancellationToken);

            return Result.NoContent();
        }
    }
}