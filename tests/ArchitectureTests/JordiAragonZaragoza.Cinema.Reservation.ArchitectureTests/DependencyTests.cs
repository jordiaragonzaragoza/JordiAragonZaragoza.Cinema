namespace JordiAragonZaragoza.Cinema.Reservation.ArchitectureTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AwesomeAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Application.Contracts.Integration.V1;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command;
    using NetArchTest.Rules;
    using Xunit;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V1;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V1;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Command.Contracts.V2;
    using JordiAragonZaragoza.Cinema.Reservation.Api.Query.Contracts.V2;

    public sealed class DependencyTests
    {
        private readonly Assembly reservationAssembly;
        private readonly IEnumerable<string> domainNamespaces;
        private readonly IEnumerable<string> applicationNamespaces;
        private readonly IEnumerable<string> applicationContractsNamespaces;
        private readonly IEnumerable<string> applicationContractsIntegrationV1Namespaces;
        private readonly IEnumerable<string> infrastructureNamespaces;
        private readonly IEnumerable<string> infrastructureEntityFrameworkNamespaces;
        private readonly IEnumerable<string> infrastructureEventStoreNamespaces;
        private readonly IEnumerable<string> apiCommandNamespaces;
        private readonly IEnumerable<string> apiQueryNamespaces;
        private readonly IEnumerable<string> apiCommandContractsV1Namespaces;
        private readonly IEnumerable<string> apiCommandContractsV2Namespaces;
        private readonly IEnumerable<string> apiQueryContractsV1Namespaces;
        private readonly IEnumerable<string> apiQueryContractsV2Namespaces;
        private readonly string[] allNamespaces;

        public DependencyTests()
        {
            this.reservationAssembly = AssemblyReference.Assembly;
            this.domainNamespaces = GetNamespacesContaining(this.reservationAssembly, "Domain");
            this.applicationNamespaces = GetNamespacesContaining(this.reservationAssembly, "Application");
            this.applicationContractsNamespaces = GetNamespacesContaining(this.reservationAssembly, "Application.Contracts");
            this.applicationContractsIntegrationV1Namespaces = GetNamespacesContaining(IntegrationV1AssemblyReference.Assembly, "Application.Contracts.Integration.V1");
            this.infrastructureNamespaces = GetNamespacesContaining(this.reservationAssembly, "Infrastructure");
            this.infrastructureEntityFrameworkNamespaces = GetNamespacesContaining(this.reservationAssembly, "Infrastructure.EntityFramework");
            this.infrastructureEventStoreNamespaces = GetNamespacesContaining(this.reservationAssembly, "Infrastructure.EventStore");

            // TODO: Complete with all Autonomous Components Namespaces
            this.apiCommandNamespaces = GetNamespacesContaining(ApiCommandAssemblyReference.Assembly, "Api.Command");
            this.apiCommandContractsV1Namespaces = GetNamespacesContaining(ApiCommandContractsV1AssemblyReference.Assembly, "Api.Command.Contracts.V1");
            this.apiCommandContractsV2Namespaces = GetNamespacesContaining(ApiCommandContractsV2AssemblyReference.Assembly, "Api.Command.Contracts.V2");
            this.apiQueryNamespaces = GetNamespacesContaining(ApiQueryAssemblyReference.Assembly, "Api.Query");
            this.apiQueryContractsV1Namespaces = GetNamespacesContaining(ApiQueryContractsV1AssemblyReference.Assembly, "Api.Query.Contracts.V1");
            this.apiQueryContractsV2Namespaces = GetNamespacesContaining(ApiQueryContractsV2AssemblyReference.Assembly, "Api.Query.Contracts.V2");

            this.allNamespaces = new List<IEnumerable<string>>
            {
                this.domainNamespaces,
                this.applicationNamespaces,
                this.applicationContractsNamespaces,
                this.applicationContractsIntegrationV1Namespaces,
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.apiCommandNamespaces,
                this.apiQueryNamespaces,
                this.apiCommandContractsV1Namespaces,
                this.apiCommandContractsV2Namespaces,
                this.apiQueryContractsV1Namespaces,
                this.apiQueryContractsV2Namespaces,
            }.SelectMany(collection => collection).ToArray();
        }

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnOtherProjects()
        {
            var namespacesCollections = new List<IEnumerable<string>>
            {
                this.applicationNamespaces,
                this.applicationContractsNamespaces,
                this.applicationContractsIntegrationV1Namespaces,
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.apiCommandNamespaces,
                this.apiQueryNamespaces,
                this.apiCommandContractsV1Namespaces,
                this.apiCommandContractsV2Namespaces,
                this.apiQueryContractsV1Namespaces,
                this.apiQueryContractsV2Namespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();

            // Act.
            var testResult = Types
                .InAssembly(this.reservationAssembly)
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
                this.applicationContractsIntegrationV1Namespaces,
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.apiCommandNamespaces,
                this.apiQueryNamespaces,
                this.apiCommandContractsV1Namespaces,
                this.apiCommandContractsV2Namespaces,
                this.apiQueryContractsV1Namespaces,
                this.apiQueryContractsV2Namespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();

            // Act.
            var testResult = Types
                .InAssembly(this.reservationAssembly)
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
        public void ApplicationContractsIntegration_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assemblyV1 = IntegrationV1AssemblyReference.Assembly;

            var namespacesCollections = new List<IEnumerable<string>>
            {
                this.domainNamespaces,
                this.applicationNamespaces,
                this.applicationContractsNamespaces,
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.apiCommandNamespaces,
                this.apiQueryNamespaces,
                this.apiCommandContractsV1Namespaces,
                this.apiCommandContractsV2Namespaces,
                this.apiQueryContractsV1Namespaces,
                this.apiQueryContractsV2Namespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();

            // Act.
            var testResult = Types
                .InAssembly(assemblyV1)
                .That()
                .ResideInNamespaceContaining("Application.Contracts.Integration")
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences.ToArray())
                .Or()
                .HaveDependencyOn(IntegrationV1AssemblyReference.Assembly?.GetName().Name ?? string.Empty)
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
                this.apiCommandNamespaces,
                this.apiQueryNamespaces,
                this.apiCommandContractsV1Namespaces,
                this.apiCommandContractsV2Namespaces,
                this.apiQueryContractsV1Namespaces,
                this.apiQueryContractsV2Namespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();

            var allowedDependencies = new List<IEnumerable<string>>
            {
                this.applicationContractsNamespaces,
                this.domainNamespaces,
            }.SelectMany(collection => collection).ToArray();

            // Act.
            var testResult = Types
                .InAssembly(this.reservationAssembly)
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
                this.apiCommandNamespaces,
                this.apiQueryNamespaces,
                this.apiCommandContractsV1Namespaces,
                this.apiCommandContractsV2Namespaces,
                this.apiQueryContractsV1Namespaces,
                this.apiQueryContractsV2Namespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();

            var allowedDependencies = new List<IEnumerable<string>>
            {
                this.applicationContractsNamespaces,
            }.SelectMany(collection => collection).ToArray();

            // Act.
            var testResult = Types
                .InAssembly(this.reservationAssembly)
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
                this.apiCommandNamespaces,
                this.apiQueryNamespaces,
                this.apiCommandContractsV1Namespaces,
                this.apiQueryContractsV1Namespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();

            var allowedDependencies = new List<IEnumerable<string>>
            {
                this.applicationContractsNamespaces,
                this.domainNamespaces,
            }.SelectMany(collection => collection).ToArray();

            // Act.
            var testResult = Types
                .InAssembly(this.reservationAssembly)
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
                this.apiCommandNamespaces,
                this.apiQueryNamespaces,
                this.apiCommandContractsV1Namespaces,
                this.apiQueryContractsV1Namespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();

            var allowedDependencies = new List<IEnumerable<string>>
            {
                this.applicationContractsNamespaces,
                this.domainNamespaces,
            }.SelectMany(collection => collection).ToArray();

            // Act.
            var testResult = Types
                .InAssembly(this.reservationAssembly)
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
        public void ApiCommandContractsV1_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assemblyV1 = ApiCommandContractsV1AssemblyReference.Assembly;

            var namespacesCollections = new List<IEnumerable<string>>
            {
                this.domainNamespaces,
                this.applicationNamespaces,
                this.applicationContractsNamespaces,
                this.applicationContractsIntegrationV1Namespaces,
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.apiCommandNamespaces,
                this.apiQueryNamespaces,
                this.apiQueryContractsV1Namespaces,
                this.apiQueryContractsV2Namespaces,
                this.apiCommandContractsV2Namespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();

            // Act.
            var testResult = Types
                .InAssembly(assemblyV1)
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences.ToArray())
                .Or()
                .HaveDependencyOn(ApiCommandContractsV1AssemblyReference.Assembly?.GetName().Name ?? string.Empty)
                .Or()
                .NotHaveDependencyOnAny(this.allNamespaces)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void ApiCommandContractsV2_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assemblyV1 = ApiCommandContractsV2AssemblyReference.Assembly;

            var namespacesCollections = new List<IEnumerable<string>>
            {
                this.domainNamespaces,
                this.applicationNamespaces,
                this.applicationContractsNamespaces,
                this.applicationContractsIntegrationV1Namespaces,
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.apiCommandNamespaces,
                this.apiQueryNamespaces,
                this.apiQueryContractsV1Namespaces,
                this.apiQueryContractsV2Namespaces,
                this.apiCommandContractsV1Namespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();

            // Act.
            var testResult = Types
                .InAssembly(assemblyV1)
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences.ToArray())
                .Or()
                .HaveDependencyOn(ApiCommandContractsV2AssemblyReference.Assembly?.GetName().Name ?? string.Empty)
                .Or()
                .NotHaveDependencyOnAny(this.allNamespaces)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void ApiQueryContractsV1_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assemblyV1 = ApiQueryContractsV1AssemblyReference.Assembly;

            var namespacesCollections = new List<IEnumerable<string>>
            {
                this.domainNamespaces,
                this.applicationNamespaces,
                this.applicationContractsNamespaces,
                this.applicationContractsIntegrationV1Namespaces,
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.apiCommandNamespaces,
                this.apiQueryNamespaces,
                this.apiCommandContractsV1Namespaces,
                this.apiCommandContractsV2Namespaces,
                this.apiQueryContractsV2Namespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();

            // Act.
            var testResult = Types
                .InAssembly(assemblyV1)
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences.ToArray())
                .Or()
                .HaveDependencyOn(ApiQueryContractsV1AssemblyReference.Assembly?.GetName().Name ?? string.Empty)
                .Or()
                .NotHaveDependencyOnAny(this.allNamespaces)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void ApiQueryContractsV2_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assemblyV1 = ApiQueryContractsV2AssemblyReference.Assembly;

            var namespacesCollections = new List<IEnumerable<string>>
            {
                this.domainNamespaces,
                this.applicationNamespaces,
                this.applicationContractsNamespaces,
                this.applicationContractsIntegrationV1Namespaces,
                this.infrastructureNamespaces,
                this.infrastructureEntityFrameworkNamespaces,
                this.infrastructureEventStoreNamespaces,
                this.apiCommandNamespaces,
                this.apiQueryNamespaces,
                this.apiCommandContractsV1Namespaces,
                this.apiCommandContractsV2Namespaces,
                this.apiQueryContractsV1Namespaces,
            };

            var forbiddenReferences = namespacesCollections.SelectMany(collection => collection).ToList();

            // Act.
            var testResult = Types
                .InAssembly(assemblyV1)
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences.ToArray())
                .Or()
                .HaveDependencyOn(ApiQueryContractsV2AssemblyReference.Assembly?.GetName().Name ?? string.Empty)
                .Or()
                .NotHaveDependencyOnAny(this.allNamespaces)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void ApiCommand_Should_Not_HaveDependencyOnOtherProjects()
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
                this.apiCommandContractsV1Namespaces,
            }.SelectMany(collection => collection).ToList();

            // Act.
            var testResult = Types
                .InAssembly(this.reservationAssembly)
                .That()
                .ResideInNamespaceContaining("Api.Command")
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences)
                .Or()
                .HaveDependencyOnAny(this.apiCommandNamespaces.ToArray())
                .Or()
                .HaveDependencyOnAny(allowedDependencies.ToArray())
                .Or()
                .NotHaveDependencyOnAny(this.allNamespaces)
                .GetResult();

            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void ApiQuery_Should_Not_HaveDependencyOnOtherProjects()
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
                this.apiCommandContractsV1Namespaces,
            }.SelectMany(collection => collection).ToList();

            // Act.
            var testResult = Types
                .InAssembly(this.reservationAssembly)
                .That()
                .ResideInNamespaceContaining("Api.Query")
                .Should()
                .NotHaveDependencyOnAny(forbiddenReferences)
                .Or()
                .HaveDependencyOnAny(this.apiCommandNamespaces.ToArray())
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

            return types.Select(type => type.Namespace)
                        .Where(@namespace => @namespace != null)
                        .Distinct()!;
        }
    }
}