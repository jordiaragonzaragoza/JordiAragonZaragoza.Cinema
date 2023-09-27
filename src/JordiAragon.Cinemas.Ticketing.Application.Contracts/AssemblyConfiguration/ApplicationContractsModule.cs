namespace JordiAragon.Cinemas.Ticketing.Application.Contracts.AssemblyConfiguration
{
    using System.Reflection;
    using JordiAragon.SharedKernel;

    public class ApplicationContractsModule : AssemblyModule
    {
        protected override Assembly CurrentAssembly => ApplicationContractsAssemblyReference.Assembly;
    }
}