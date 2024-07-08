namespace JordiAragon.Cinema.Reservation.TestUtilities.Domain
{
    using System;
    using JordiAragon.Cinema.Reservation.Auditorium.Domain;

    public static partial class Constants
    {
        public static class Auditorium
        {
            public const string Name = "Auditorium One";
            public static readonly AuditoriumId Id = AuditoriumId.Create(new Guid("c91aa0e0-9bc0-4db3-805c-23e3d8eabf53"));
            public static readonly ushort Rows = 10;
            public static readonly ushort SeatsPerRow = 10;
        }
    }
}