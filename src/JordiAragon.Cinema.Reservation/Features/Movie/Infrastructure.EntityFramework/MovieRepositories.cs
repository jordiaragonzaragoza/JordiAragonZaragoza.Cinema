namespace JordiAragon.Cinema.Reservation.Movie.Infrastructure.EntityFramework
{
    using Autofac;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragon.Cinema.Reservation.Movie.Domain;
    using JordiAragon.SharedKernel.Contracts.Repositories;

    public static class MovieRepositories
    {
        public static void RegisterBusinessModelRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<ReservationRepository<Movie, MovieId>>()
                                .As<IRepository<Movie, MovieId>>()
                                .InstancePerLifetimeScope();

            builder.RegisterType<ReservationRepository<Movie, MovieId>>()
                    .As<IReadRepository<Movie, MovieId>>()
                    .InstancePerLifetimeScope();

            builder.RegisterType<ReservationRepository<Movie, MovieId>>()
                    .As<IReadListRepository<Movie, MovieId>>()
                    .InstancePerLifetimeScope();

            builder.RegisterType<ReservationRepository<Movie, MovieId>>()
                    .As<ISpecificationReadRepository<Movie, MovieId>>()
                    .InstancePerLifetimeScope();
        }
    }
}