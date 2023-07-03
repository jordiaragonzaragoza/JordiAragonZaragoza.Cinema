namespace JordiAragon.Cinema.Presentation.WebApi.Mappers.V2
{
    using AutoMapper;
    using JordiAragon.Cinema.Presentation.WebApi.Controllers.V2;

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
