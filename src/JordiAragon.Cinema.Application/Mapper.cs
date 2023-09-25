namespace JordiAragon.Cinema.Application
{
    using AutoMapper;
    using JordiAragon.Cinema.Application.Auditorium.Queries;
    using JordiAragon.Cinema.Application.Movie.Queries;
    using JordiAragon.Cinema.Application.Showtime.Queries;

    public class Mapper : Profile
    {
        public Mapper()
        {
            this.MapAuditorium();
            this.MapMovie();
            this.MapShowtime();
        }
    }
}
