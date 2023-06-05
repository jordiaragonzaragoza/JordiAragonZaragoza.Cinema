namespace JordiAragon.Cinema.Infrastructure.AssemblyConfiguration
{
    using System.Reflection;
    using JordiAragon.SharedKernel;

    public class InfrastructureModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => InfrastructureAssemblyReference.Assembly;
    }
}