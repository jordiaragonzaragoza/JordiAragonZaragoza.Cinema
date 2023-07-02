namespace JordiAragon.Cinema.Presentation.WebApi.Mappers.V1
{
    using AutoMapper;
    using JordiAragon.Cinema.Presentation.WebApi.Controllers.V1;

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
