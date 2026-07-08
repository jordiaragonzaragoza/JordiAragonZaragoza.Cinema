namespace JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain
{
    using System;
    using JordiAragonZaragoza.Cinema.Reservation.Partition.Domain;

    public static partial class Constants
    {
        public static class Partition
        {
            public static readonly PartitionId Id = new(new Guid("5d6e7f8a-9b0c-1d2e-3f4a-5b6c7d8e9f0a"));
        }
    }
}
