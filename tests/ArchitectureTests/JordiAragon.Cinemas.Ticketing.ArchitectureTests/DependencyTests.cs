namespace JordiAragon.Cinemas.Ticketing.ArchitectureTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2;
    using NetArchTest.Rules;
    using Xunit;

    public class DependencyTests
    {
        private readonly Assembly assembly;
        private readonly IEnumerable<string> domainNamespaces;
        private readonly IEnumerable<string> applicationNamespaces;
        private readonly IEnumerable<string> applicationContractsNamespaces;
        private readonly IEnumerable<string> applicationContractsIntegrationMessagesNamespaces;
        private readonly IEnumerable<string> infrastructureNamespaces;
        private readonly IEnumerable<string> infrastructureEntityFrameworkNamespaces;
        private readonly IEnumerable<string> infrastructureEventStoreNamespaces;
        private readonly IEnumerable<string> webApiNamespaces;
        private readonly string webApiContractsV1Namespace = WebApiContractsV1AssemblyReference.Assembly.GetName().Name;
        private readonly string webApiContractsV2Namespace = WebApiContractsV2AssemblyReference.Assembly.GetName().Name;

        public DependencyTests()
        {
            this.assembly = AssemblyReference.Assembly;
            this.domainNamespaces = GetNamespacesContaining(this.assembly, "Domain");
            this.applicationNamespaces = GetNamespacesContaining(this.assembly, "Application");
            this.applicationContractsNamespaces = GetNamespacesContaining(this.assembly, "Application.Contracts");
            this.applicationContractsIntegrationMessagesNamespaces = GetNamespacesContaining(this.assembly, "Application.Contracts.IntegrationMessages");
            this.infrastructureNamespaces = GetNamespacesContaining(this.assembly, "Infrastructure");
            this.infrastructureEntityFrameworkNamespaces = GetNamespacesContaining(this.assembly, "Infrastructure.EntityFramework");
            this.infrastructureEventStoreNamespaces = GetNamespacesContaining(this.assembly, "Infrastructure.EventStore");
            this.webApiNamespaces = GetNamespacesContaining(this.assembly, "Presentation.WebApi");
        }

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnOtherProjects()
        {
            var namespacesCollections = new List<IEnumerable<string>>
            {
                this.applicationNamespaces,
                this.applicationContractsNamespaces,
                this.applicationContractsIntegrationMessagesNamespaces,
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.webApiNamespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();
            forbiddenReferences.Add(this.webApiContractsV1Namespace);
            forbiddenReferences.Add(this.webApiContractsV2Namespace);

            // Act.
            var testResult = Types
                .InAssembly(this.assembly)
                .That()
                .ResideInNamespaceContaining("Domain")
                .ShouldNot()
                .HaveDependencyOnAny(forbiddenReferences.ToArray())
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void ApplicationContracts_Should_Not_HaveDependencyOnOtherProjects()
        {
            var namespacesCollections = new List<IEnumerable<string>>
            {
                this.domainNamespaces,
                this.applicationNamespaces,
                this.applicationContractsIntegrationMessagesNamespaces,
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.webApiNamespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();
            forbiddenReferences.Add(this.webApiContractsV1Namespace);
            forbiddenReferences.Add(this.webApiContractsV2Namespace);

            forbiddenReferences.RemoveAll(item => item.Contains("Application.Contracts"));

            // Act.
            var testResult = Types
                .InAssembly(this.assembly)
                .That()
                .ResideInNamespaceContaining("Application.Contracts")
                .ShouldNot()
                .HaveDependencyOnAny(forbiddenReferences.ToArray())
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        private static IEnumerable<string> GetNamespacesContaining(Assembly assembly, string fragmentNamespace)
        {
            var types = Types
                .InAssembly(assembly)
                .That()
                .ResideInNamespaceContaining(fragmentNamespace)
                .GetTypes();

            return types.Select(type => type.Namespace).Distinct();
        }
    }
}