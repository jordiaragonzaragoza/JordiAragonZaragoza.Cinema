namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain.Rules
{
    using System.Linq;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public class OnlyPossibleToAssignPermissionOnceRule : IBusinessRule
    {
        private readonly Assignment assignment;
        private readonly Permission permission;

        public OnlyPossibleToAssignPermissionOnceRule(
            Assignment assignment,
            Permission permission)
        {
            this.assignment = assignment;
            this.permission = permission;
        }

        public string Message => "Only possible to assign a permission once per scope.";

        public bool IsBroken()
        {
            return this.assignment.Permissions.Contains(this.permission);
        }
    }
}