namespace JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;

    public static partial class Constants
    {
        public static class User
        {
            public static readonly UserId Id = UserId.Create(new Guid("08ffddf5-3826-483f-a806-b3144477c7e8"));
        }
    }
}