namespace JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2.User
{
    public static class UserRoutes
    {
        public const string GrantUser = $"{Base}/{{userId}}/grant";

        public const string RevokeUser = $"{Base}/{{userId}}/revoke";

        public const string AssignRole = $"{Base}/{{userId}}/roles/assign";

        public const string RemoveRole = $"{Base}/{{userId}}/roles/remove";

        public const string AssignPermission = $"{Base}/{{userId}}/permissions/assign";

        public const string RemovePermission = $"{Base}/{{userId}}/permissions/remove";

        private const string Base = "users";
    }
}