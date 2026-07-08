namespace JordiAragonZaragoza.Cinema.Reservation.User.Domain.Rules
{
    using System.Linq;
    using JordiAragonZaragoza.SharedKernel.Domain.Contracts.Interfaces;

    public class OnlyPossibleToAssignRoleOnceRule : IBusinessRule
    {
        private readonly Assignment assignment;
        private readonly Role role;

        public OnlyPossibleToAssignRoleOnceRule(
            Assignment assignment,
            Role role)
        {
            this.assignment = assignment;
            this.role = role;
        }

        public string Message => "Only possible to assign a role once per scope.";

        public bool IsBroken()
        {
            return this.assignment.Roles.Contains(this.role);
        }
    }
}