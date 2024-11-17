namespace JordiAragonZaragoza.Cinema.Reservation.User.Infrastructure.EntityFramework
{
    using Autofac;
    using JordiAragonZaragoza.Cinema.Reservation.User.Domain;
    using JordiAragonZaragoza.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragonZaragoza.SharedKernel.Contracts.Repositories;

    public static class UserRepositories
    {
        public static void RegisterBusinessModelRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<ReservationRepository<User, UserId>>()
                    .As<IRepository<User, UserId>>()
                    .InstancePerLifetimeScope();

            builder.RegisterType<ReservationRepository<User, UserId>>()
                    .As<IReadRepository<User, UserId>>()
                    .InstancePerLifetimeScope();

            builder.RegisterType<ReservationRepository<User, UserId>>()
                    .As<IReadListRepository<User, UserId>>()
                    .InstancePerLifetimeScope();

            builder.RegisterType<ReservationRepository<User, UserId>>()
                    .As<ISpecificationReadRepository<User, UserId>>()
                    .InstancePerLifetimeScope();
        }
    }
}