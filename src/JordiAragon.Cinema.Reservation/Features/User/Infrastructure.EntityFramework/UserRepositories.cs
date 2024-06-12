namespace JordiAragon.Cinema.Reservation.Features.User.Infrastructure.EntityFramework
{
    using Autofac;
    using JordiAragon.Cinema.Reservation.User.Domain;
    using JordiAragon.Cinema.Reservation.Common.Infrastructure.EntityFramework.Repositories.BusinessModel;
    using JordiAragon.SharedKernel.Contracts.Repositories;

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