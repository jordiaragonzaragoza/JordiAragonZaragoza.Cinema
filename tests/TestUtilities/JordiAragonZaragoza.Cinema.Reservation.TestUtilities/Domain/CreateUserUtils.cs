namespace JordiAragonZaragoza.Cinema.Reservation.TestUtilities.Domain
{
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;

    public static class CreateUserUtils
    {
        public static User Create()
            => User.Create(
                Constants.User.Id);
    }
}