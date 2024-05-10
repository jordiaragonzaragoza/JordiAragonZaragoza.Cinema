namespace JordiAragon.Cinema.Reservation.ArchitectureTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FluentAssertions;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V1;
    using JordiAragon.Cinema.Reservation.Presentation.HttpRestfulApi.Contracts.V2;
    using NetArchTest.Rules;
    using Xunit;

    public sealed class DependencyTests
    {
        private readonly Assembly assembly;
        private readonly IEnumerable<string> domainNamespaces;
        private readonly IEnumerable<string> applicationNamespaces;
        private readonly IEnumerable<string> applicationContractsNamespaces;
        private readonly IEnumerable<string> applicationContractsIntegrationMessagesNamespaces;
        private readonly IEnumerable<string> infrastructureNamespaces;
        private readonly IEnumerable<string> infrastructureEntityFrameworkNamespaces;
        private readonly IEnumerable<string> infrastructureEventStoreNamespaces;
        private readonly IEnumerable<string> httpRestfulApiNamespaces;
        private readonly string[] allNamespaces;
        private readonly string httpRestfulApiContractsV1Namespace = HttpRestfulApiContractsV1AssemblyReference.Assembly.GetName().Name;
        private readonly string httpRestfulApiContractsV2Namespace = HttpRestfulApiContractsV2AssemblyReference.Assembly.GetName().Name;

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
            this.httpRestfulApiNamespaces = GetNamespacesContaining(this.assembly, "Presentation.HttpRestfulApi");

            this.allNamespaces = new List<IEnumerable<string>>
            {
                this.domainNamespaces,
                this.applicationNamespaces,
                this.applicationContractsNamespaces,
                this.applicationContractsIntegrationMessagesNamespaces,
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.httpRestfulApiNamespaces,
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
                this.httpRestfulApiNamespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();
            forbiddenReferences.Add(this.httpRestfulApiContractsV1Namespace);
            forbiddenReferences.Add(this.httpRestfulApiContractsV2Namespace);

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
                this.httpRestfulApiNamespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();
            forbiddenReferences.Add(this.httpRestfulApiContractsV1Namespace);
            forbiddenReferences.Add(this.httpRestfulApiContractsV2Namespace);

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
                this.httpRestfulApiNamespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();
            forbiddenReferences.Add(this.httpRestfulApiContractsV1Namespace);
            forbiddenReferences.Add(this.httpRestfulApiContractsV2Namespace);

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
                this.httpRestfulApiNamespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();
            forbiddenReferences.Add(this.httpRestfulApiContractsV1Namespace);
            forbiddenReferences.Add(this.httpRestfulApiContractsV2Namespace);

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
                this.httpRestfulApiNamespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();
            forbiddenReferences.Add(this.httpRestfulApiContractsV1Namespace);
            forbiddenReferences.Add(this.httpRestfulApiContractsV2Namespace);

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
                this.httpRestfulApiNamespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();
            forbiddenReferences.Add(this.httpRestfulApiContractsV1Namespace);
            forbiddenReferences.Add(this.httpRestfulApiContractsV2Namespace);

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

        [Fact]
        public void InfrastructureEventStore_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var namespacesCollections = new List<IEnumerable<string>>
            {
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.httpRestfulApiNamespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();
            forbiddenReferences.Add(this.httpRestfulApiContractsV1Namespace);
            forbiddenReferences.Add(this.httpRestfulApiContractsV2Namespace);

            var allowedDependencies = new List<IEnumerable<string>>
            {
                this.applicationContractsNamespaces,
                this.domainNamespaces,
            }.SelectMany(collection => collection).ToArray();

            // Act.
            var testResult = Types
                .InAssembly(this.assembly)
                .That()
                .ResideInNamespaceContaining("Infrastructure.EventStore")
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences.ToArray())
                .Or()
                .HaveDependencyOnAny(this.infrastructureEventStoreNamespaces.ToArray())
                .Or()
                .HaveDependencyOnAny(allowedDependencies)
                .Or()
                .NotHaveDependencyOnAny(this.allNamespaces)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void HttpRestfulApiContractsV1_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assemblyV1 = HttpRestfulApiContractsV1AssemblyReference.Assembly;

            var namespacesCollections = new List<IEnumerable<string>>
            {
                this.domainNamespaces,
                this.applicationNamespaces,
                this.applicationContractsNamespaces,
                this.applicationContractsIntegrationMessagesNamespaces,
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.httpRestfulApiNamespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();
            forbiddenReferences.Add(this.httpRestfulApiContractsV2Namespace);

            // Act.
            var testResult = Types
                .InAssembly(assemblyV1)
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences.ToArray())
                .Or()
                .HaveDependencyOn(this.httpRestfulApiContractsV1Namespace)
                .Or()
                .NotHaveDependencyOnAny(this.allNamespaces)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void HttpRestfulApiContractsV2_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assemblyV2 = HttpRestfulApiContractsV2AssemblyReference.Assembly;

            var namespacesCollections = new List<IEnumerable<string>>
            {
                this.domainNamespaces,
                this.applicationNamespaces,
                this.applicationContractsNamespaces,
                this.applicationContractsIntegrationMessagesNamespaces,
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.httpRestfulApiNamespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();
            forbiddenReferences.Add(this.httpRestfulApiContractsV1Namespace);

            // Act.
            var testResult = Types
                .InAssembly(assemblyV2)
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences.ToArray())
                .Or()
                .HaveDependencyOn(this.httpRestfulApiContractsV1Namespace)
                .Or()
                .NotHaveDependencyOnAny(this.allNamespaces)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void HttpRestfulApi_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var forbiddenReferences = new List<IEnumerable<string>>
            {
                this.domainNamespaces,
                this.applicationNamespaces,
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
            }.SelectMany(collection => collection).ToArray();

            var allowedDependencies = new List<IEnumerable<string>>
            {
                this.applicationContractsNamespaces,
            }.SelectMany(collection => collection).ToList();
            allowedDependencies.Add(this.httpRestfulApiContractsV1Namespace);
            allowedDependencies.Add(this.httpRestfulApiContractsV2Namespace);

            // Act.
            var testResult = Types
                .InAssembly(this.assembly)
                .That()
                .ResideInNamespaceContaining("Presentation.HttpRestfulApi")
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences)
                .Or()
                .HaveDependencyOnAny(this.httpRestfulApiNamespaces.ToArray())
                .Or()
                .HaveDependencyOnAny(allowedDependencies.ToArray())
                .Or()
                .NotHaveDependencyOnAny(this.allNamespaces)
                .GetResult();

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