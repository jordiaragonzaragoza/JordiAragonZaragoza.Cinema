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
    using Microsoft.Extensions.Logging;

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
        private readonly ILogger<ReservationOwnerOrAdminPolicy> logger;

        public ReservationOwnerOrAdminPolicy(
            IReadRepository<ReservationReadModel, Guid> reservationRepository,
            ILogger<ReservationOwnerOrAdminPolicy> logger)
        {
            this.reservationRepository = reservationRepository
                ?? throw new ArgumentNullException(nameof(reservationRepository));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string Name => Policies.ReservationOwnerOrAdmin;

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
                this.logger.LogWarning(
                    "Authorization failed for user {UserId} because ReservationOwnerOrAdminPolicy requires a resourceId and user is not an Admin.",
                    currentUserId);

                return Result.Forbidden("Access denied.");
            }

            var reservation = await this.reservationRepository.GetByIdAsync(resourceId.Value, cancellationToken);
            if (reservation is null)
            {
                this.logger.LogWarning(
                    "Authorization failed for user {UserId} because ReservationOwnerOrAdminPolicy requires a Reservation {ResourceId} and it was not found.",
                    currentUserId,
                    resourceId);

                return Result.Forbidden("Access denied.");
            }

            if (reservation.UserId != currentUserId)
            {
                this.logger.LogWarning(
                    "Authorization failed for user {UserId} on ReservationOwnerOrAdminPolicy because User is neither the reservation owner nor an Admin.",
                    currentUserId);

                return Result.Forbidden("Access denied.");
            }

            return Result.Success();
        }
    }
}