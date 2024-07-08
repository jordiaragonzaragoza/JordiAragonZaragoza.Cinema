namespace JordiAragon.Cinema.Reservation.TestUtilities.Domain
{
    using JordiAragon.Cinema.Reservation.User.Domain;

    public static class CreateUserUtils
    {
        public static User Create()
            => User.Create(
                Constants.User.Id);
    }
}