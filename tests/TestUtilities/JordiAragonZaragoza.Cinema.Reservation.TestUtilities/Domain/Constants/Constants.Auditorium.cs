namespace JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;

    public static partial class Constants
    {
        public static class Auditorium
        {
            public const string Name = "Auditorium One";
            public static readonly AuditoriumId Id = new AuditoriumId(new Guid("c91aa0e0-9bc0-4db3-805c-23e3d8eabf53"));
            public static readonly Rows Rows = Rows.Create(10);
            public static readonly SeatsPerRow SeatsPerRow = SeatsPerRow.Create(10);
        }
    }
}