namespace JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain
{
    using JordiAragonZaragoza.Cinema.Reservation.Common.Application;

    public static partial class Constants
    {
        public static class Role
        {
            public static readonly global::JordiAragonZaragoza.Cinema.Reservation.User.Domain.Role Admin = global::JordiAragonZaragoza.Cinema.Reservation.User.Domain.Role.Create(Roles.Admin);
            public static readonly global::JordiAragonZaragoza.Cinema.Reservation.User.Domain.Role Viewer = global::JordiAragonZaragoza.Cinema.Reservation.User.Domain.Role.Create(Roles.Viewer);
        }
    }
}
