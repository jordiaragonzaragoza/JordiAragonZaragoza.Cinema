namespace JordiAragon.Cinemas.Ticketing.Domain.AssemblyConfiguration
{
    using System.Reflection;
    using JordiAragon.SharedKernel;

    public class DomainModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => DomainAssemblyReference.Assembly;
    }
}