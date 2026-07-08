namespace JordiAragonZaragoza.Cinema.Reservation.User.Application.Contracts
{
    public static class UserPermisions
    {
        public const string AssignPermission = "assignPermission:user";
        public const string AssignRole = "assignRole:user";
        public const string CreateUser = "createUser:user";
        public const string GrantUser = "grantUser:user";
        public const string RemovePermission = "removePermission:user";
        public const string RemoveRole = "removeRole:user";
        public const string RemoveUser = "removeUser:user";
        public const string RevokeUser = "revokeUser:user";
        public const string GetUserAuthorization = "getUserAuthorization:user";
        public const string GetUserReservation = "getUserReservation:user";
        public const string GetUserReservations = "getUserReservations:user";
    }
}
