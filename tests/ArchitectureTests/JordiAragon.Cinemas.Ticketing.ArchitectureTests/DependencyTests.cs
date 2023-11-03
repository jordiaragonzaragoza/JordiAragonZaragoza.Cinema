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
        private readonly string[] allNamespaces;
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

            this.allNamespaces = new List<IEnumerable<string>>
            {
                this.domainNamespaces,
                this.applicationNamespaces,
                this.applicationContractsNamespaces,
                this.applicationContractsIntegrationMessagesNamespaces,
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.webApiNamespaces,
            }.SelectMany(collection => collection).ToArray();
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
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences.ToArray())
                .Or()
                .HaveDependencyOnAny(this.domainNamespaces.ToArray())
                .Or()
                .NotHaveDependencyOnAny(this.allNamespaces)
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

            // Act.
            var testResult = Types
                .InAssembly(this.assembly)
                .That()
                .ResideInNamespaceContaining("Application.Contracts")
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences.ToArray())
                .Or()
                .HaveDependencyOnAny(this.applicationContractsNamespaces.ToArray())
                .Or()
                .NotHaveDependencyOnAny(this.allNamespaces)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void ApplicationContractsIntegrationMessages_Should_Not_HaveDependencyOnOtherProjects()
        {
            var namespacesCollections = new List<IEnumerable<string>>
            {
                this.domainNamespaces,
                this.applicationNamespaces,
                this.applicationContractsNamespaces,
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
                .ResideInNamespaceContaining("Application.Contracts.IntegrationMessages")
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences.ToArray())
                .Or()
                .HaveDependencyOnAny(this.applicationContractsIntegrationMessagesNamespaces.ToArray())
                .Or()
                .NotHaveDependencyOnAny(this.allNamespaces)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void Application_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var namespacesCollections = new List<IEnumerable<string>>
            {
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.webApiNamespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();
            forbiddenReferences.Add(this.webApiContractsV1Namespace);
            forbiddenReferences.Add(this.webApiContractsV2Namespace);

            var allowedDependencies = new List<IEnumerable<string>>
            {
                this.applicationContractsNamespaces,
                this.domainNamespaces,
            }.SelectMany(collection => collection).ToArray();

            // Act.
            var testResult = Types
                .InAssembly(this.assembly)
                .That()
                .ResideInNamespaceContaining("Application")
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences.ToArray())
                .Or()
                .HaveDependencyOnAny(this.applicationNamespaces.ToArray())
                .Or()
                .HaveDependencyOnAny(allowedDependencies)
                .Or()
                .NotHaveDependencyOnAny(this.allNamespaces)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var namespacesCollections = new List<IEnumerable<string>>
            {
                this.domainNamespaces,
                this.applicationNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.webApiNamespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();
            forbiddenReferences.Add(this.webApiContractsV1Namespace);
            forbiddenReferences.Add(this.webApiContractsV2Namespace);

            var allowedDependencies = new List<IEnumerable<string>>
            {
                this.applicationContractsNamespaces,
            }.SelectMany(collection => collection).ToArray();

            // Act.
            var testResult = Types
                .InAssembly(this.assembly)
                .That()
                .ResideInNamespaceContaining("Infrastructure")
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences.ToArray())
                .Or()
                .HaveDependencyOnAny(this.infrastructureNamespaces.ToArray())
                .Or()
                .HaveDependencyOnAny(allowedDependencies)
                .Or()
                .NotHaveDependencyOnAny(this.allNamespaces)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void InfrastructureEntityFramework_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var namespacesCollections = new List<IEnumerable<string>>
            {
                this.infrastructureNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.webApiNamespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();
            forbiddenReferences.Add(this.webApiContractsV1Namespace);
            forbiddenReferences.Add(this.webApiContractsV2Namespace);

            var allowedDependencies = new List<IEnumerable<string>>
            {
                this.applicationContractsNamespaces,
                this.domainNamespaces,
            }.SelectMany(collection => collection).ToArray();

            // Act.
            var testResult = Types
                .InAssembly(this.assembly)
                .That()
                .ResideInNamespaceContaining("Infrastructure.EntityFramework")
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences.ToArray())
                .Or()
                .HaveDependencyOnAny(this.infrastructureEntityFrameworkNamespaces.ToArray())
                .Or()
                .HaveDependencyOnAny(allowedDependencies)
                .Or()
                .NotHaveDependencyOnAny(this.allNamespaces)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        /*
        [Fact]
        public void InfrastructureEventStore_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = InfrastructureEventStoreAssemblyReference.Assembly;

            var forbiddenDependencies = new[]
            {
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.webApiNamespace,
                this.webApiContractsNamespace,
            };

            var dependencies = new[]
            {
                this.applicationContractsNamespace,
                this.sharedKernelNamespace,
                this.domainNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny(forbiddenDependencies)
                .And()
                .HaveDependencyOn(this.infrastructureEventStoreNamespace)
                .Or()
                .HaveDependencyOnAny(dependencies)
                .Or()
                .NotHaveDependencyOnAny(this.allProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void WebApiContracts_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = WebApiContractsAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.sharedKernelNamespace,
                this.sharedKernelContractsNamespace,
                this.domainNamespace,
                this.domainContractsNamespace,
                this.applicationNamespace,
                this.applicationContractsNamespace,
                this.applicationContractsIntegrationMessagesNamespace,
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.infrastructureEventStoreNamespace,
                this.webApiNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny(otherProjects)
                .Or()
                .HaveDependencyOn(this.webApiContractsNamespace)
                .Or()
                .NotHaveDependencyOnAny(this.allProjects)
                .GetResult();

            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void WebApi_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = WebApiAssemblyReference.Assembly;

            var forbiddenDependencies = new[]
            {
                this.domainNamespace,
                this.domainContractsNamespace,
                this.applicationNamespace,
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.infrastructureEventStoreNamespace,
            };

            var dependencies = new[]
            {
                this.applicationContractsNamespace,
                this.sharedKernelNamespace,
                this.webApiContractsNamespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .Should()
                .NotHaveDependencyOnAny(forbiddenDependencies)
                .Or()
                .HaveDependencyOn(this.webApiNamespace)
                .Or()
                .HaveDependencyOnAny(dependencies)
                .Or()
                .NotHaveDependencyOnAny(this.allProjects)
                .GetResult();

            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }
         */

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