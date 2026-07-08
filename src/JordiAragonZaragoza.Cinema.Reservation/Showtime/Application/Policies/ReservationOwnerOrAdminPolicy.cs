namespace JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Policies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Ardalis.Result;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Application;
    using JordiAragonZaragoza.Cinema.Reservation.Showtime.Application.Contracts.ReadModels;
    using JordiAragonZaragoza.SharedKernel.Application.Contracts.Interfaces;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    /// <summary>
    /// Passes when the current user owns the reservation identified by
    /// resourceId, or holds the Admin role. Loads ReservationReadModel
    /// (not the Showtime aggregate) to resolve ownership cheaply — this
    /// is an authorization-time lookup, not a domain operation, so the
    /// read model is the correct source, not event replay via the aggregate.
    /// </summary>
    public sealed class ReservationOwnerOrAdminPolicy : IAuthorizationPolicy
    {
        private readonly IReadRepository<ReservationReadModel, Guid> reservationRepository;

        public ReservationOwnerOrAdminPolicy(
            IReadRepository<ReservationReadModel, Guid> reservationRepository)
        {
            this.reservationRepository = reservationRepository
                ?? throw new ArgumentNullException(nameof(reservationRepository));
        }

        public string Name => Policies.SelfOrAdmin;

        public async Task<Result> EvaluateAsync(
            Guid currentUserId,
            IReadOnlyCollection<string> currentUserRoles,
            IReadOnlyCollection<string> currentUserPermissions,
            Guid? resourceId,
            CancellationToken cancellationToken = default)
        {
            if (currentUserRoles.Contains(Roles.Admin))
            {
                return Result.Success();
            }

            if (resourceId is null)
            {
                // Fail-closed: without resourceId and without Admin, there is no basis for authorization.
                return Result.Forbidden(
                    $"Policy '{Policies.SelfOrAdmin}' requires a resourceId " +
                    $"when the user is not an Admin.");
            }

            var reservation = await this.reservationRepository.GetByIdAsync(resourceId.Value, cancellationToken);
            if (reservation is null)
            {
                return Result.NotFound($"Reservation {resourceId} not found.");
            }

            return reservation.UserId == currentUserId
                ? Result.Success()
                : Result.Forbidden("User is neither the reservation owner nor an Admin.");
        }
    }
}