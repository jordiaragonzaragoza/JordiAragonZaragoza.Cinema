namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain
{
    using System;
    using JordiAragonZaragoza.SharedKernel.Domain.ValueObjects;

    public sealed class AssignmentId : BaseEntityId<Guid>
    {
        public AssignmentId(Guid value)
            : base(value)
        {
        }
    }
}