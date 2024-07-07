namespace JordiAragon.Cinema.Reservation.TestUtilities.Domain
{
    using JordiAragon.Cinema.Reservation.Movie.Domain;

    public static class CreateMovieUtils
    {
        public static Movie Create()
            => Movie.Add(
                Constants.Movie.Id,
                Constants.Movie.Title,
                Constants.Movie.Runtime,
                Constants.Movie.ExhibitionPeriod);
    }
}