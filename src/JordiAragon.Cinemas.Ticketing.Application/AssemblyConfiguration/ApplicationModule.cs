namespace JordiAragon.Cinemas.Ticketing.Application.AssemblyConfiguration
{
    using System.Reflection;
    using Autofac;
    using JordiAragon.SharedKernel;

    public class ApplicationModule : AssemblyModule
    {
        private readonly bool isDevelopment = false;

        public ApplicationModule(bool isDevelopment)
        {
            this.isDevelopment = isDevelopment;
        }

        protected override Assembly CurrentAssembly => ApplicationAssemblyReference.Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            if (this.isDevelopment)
            {
                RegisterDevelopmentOnlyDependencies(builder);
            }
            else
            {
                RegisterProductionOnlyDependencies(builder);
            }

            RegisterCommonDependencies(builder);
        }

        private static void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
        {
            // NOTE: Add any development only services here
        }

        private static void RegisterProductionOnlyDependencies(ContainerBuilder builder)
        {
            // NOTE: Add any production only services here
        }

        /// <summary>
        /// Here we can registrate ApplicationServices.
        /// </summary>
        /// <param name="builder">The Container Builder.</param>
        private static void RegisterCommonDependencies(ContainerBuilder builder)
        {
            // NOTE: Add any common services here.
        }
    }
}