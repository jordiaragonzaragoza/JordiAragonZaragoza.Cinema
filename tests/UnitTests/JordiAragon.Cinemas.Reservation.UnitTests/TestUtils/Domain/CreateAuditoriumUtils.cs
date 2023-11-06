namespace JordiAragon.Cinemas.Reservation.UnitTests.TestUtils.Domain
{
    using JordiAragon.Cinemas.Reservation.Auditorium.Domain;

    public static class CreateAuditoriumUtils
    {
        public static Auditorium Create()
            => Auditorium.Create(
                Constants.Auditorium.Id,
                Constants.Auditorium.Rows,
                Constants.Auditorium.SeatsPerRow);
    }
}