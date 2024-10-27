namespace JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain
{
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;

    public static class CreateAuditoriumUtils
    {
        public static Auditorium Create()
            => Auditorium.Create(
                Constants.Auditorium.Id,
                Constants.Auditorium.Name,
                Constants.Auditorium.Rows,
                Constants.Auditorium.SeatsPerRow);
    }
}