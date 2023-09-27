namespace JordiAragon.Cinemas.Ticketing.ArchitectureTests
{
    using FluentAssertions;
    using JordiAragon.Cinemas.Ticketing.Application;
    using JordiAragon.Cinemas.Ticketing.Application.Contracts;
    using JordiAragon.Cinemas.Ticketing.Domain;
    using JordiAragon.Cinemas.Ticketing.Infrastructure;
    using JordiAragon.Cinemas.Ticketing.Infrastructure.EntityFramework;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V1;
    using JordiAragon.Cinemas.Ticketing.Presentation.WebApi.Contracts.V2;
    using JordiAragon.SharedKernel;
    using JordiAragon.SharedKernel.Domain.Contracts;
    using NetArchTest.Rules;
    using Xunit;

    public class ArchitectureTests
    {
        private readonly string domainNamespace = DomainAssemblyReference.Assembly.GetName().Name;
        private readonly string domainSharedNamespace = DomainContractsAssemblyReference.Assembly.GetName().Name;
        private readonly string applicationNamespace = ApplicationAssemblyReference.Assembly.GetName().Name;
        private readonly string applicationContractsNamespace = ApplicationContractsAssemblyReference.Assembly.GetName().Name;
        private readonly string infrastructureNamespace = InfrastructureAssemblyReference.Assembly.GetName().Name;
        private readonly string infrastructureEntityFrameworkNamespace = InfrastructureEntityFrameworkAssemblyReference.Assembly.GetName().Name;
        private readonly string webApiNamespace = WebApiAssemblyReference.Assembly.GetName().Name;
        private readonly string webApiContractsV1Namespace = WebApiContractsV1AssemblyReference.Assembly.GetName().Name;
        private readonly string webApiContractsV2Namespace = WebApiContractsV2AssemblyReference.Assembly.GetName().Name;

        [Fact]
        public void SharedKernel_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = SharedKernelAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.domainNamespace,
                this.domainSharedNamespace,
                this.applicationNamespace,
                this.applicationContractsNamespace,
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.webApiNamespace,
                this.webApiContractsV1Namespace,
                this.webApiContractsV2Namespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(GetFailingTypes(testResult));
        }

        [Fact]
        public void DomainShared_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = DomainContractsAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.applicationNamespace,
                this.applicationContractsNamespace,
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.webApiNamespace,
                this.webApiContractsV1Namespace,
                this.webApiContractsV2Namespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(GetFailingTypes(testResult));
        }

        [Fact]
        public void Domain_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = DomainAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.applicationNamespace,
                this.applicationContractsNamespace,
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.webApiNamespace,
                this.webApiContractsV1Namespace,
                this.webApiContractsV2Namespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(GetFailingTypes(testResult));
        }

        [Fact]
        public void Application_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = ApplicationAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.webApiNamespace,
                this.webApiContractsV1Namespace,
                this.webApiContractsV2Namespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(GetFailingTypes(testResult));
        }

        [Fact]
        public void Handlers_Should_Have_DependencyOnArdalisResult()
        {
            // Arrange.
            var assembly = ApplicationAssemblyReference.Assembly;

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("Handler")
                .And().DoNotHaveNameEndingWith("EventHandler")
                .And().DoNotHaveNameEndingWith("NotificationHandler")
                .Should()
                .HaveDependencyOn("Ardalis.Result")
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(GetFailingTypes(testResult));
        }

        [Fact]
        public void ApplicationContacts_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = ApplicationContractsAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.infrastructureNamespace,
                this.infrastructureEntityFrameworkNamespace,
                this.webApiNamespace,
                this.webApiContractsV1Namespace,
                this.webApiContractsV2Namespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(GetFailingTypes(testResult));
        }

        [Fact]
        public void Infrastructure_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = InfrastructureAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.infrastructureEntityFrameworkNamespace,
                this.webApiNamespace,
                this.webApiContractsV1Namespace,
                this.webApiContractsV2Namespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(GetFailingTypes(testResult));
        }

        [Fact]
        public void InfrastructureEntityFramework_Should_Not_HaveDependencyOnOtherProjects()
        {
            // Arrange.
            var assembly = InfrastructureEntityFrameworkAssemblyReference.Assembly;

            var otherProjects = new[]
            {
                this.webApiNamespace,
                this.webApiContractsV1Namespace,
                this.webApiContractsV2Namespace,
            };

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAny(otherProjects)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(GetFailingTypes(testResult));
        }

        [Fact]
        public void ControllerBase_Should_HaveDependencyOnMediatR()
        {
            // Arrange.
            var assembly = WebApiAssemblyReference.Assembly;

            // Act.
            var testResult = Types
                .InAssembly(assembly)
                .That()
                .HaveNameEndingWith("ControllerBase")
                .Should()
                .HaveDependencyOn("MediatR")
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(GetFailingTypes(testResult));
        }

        private static string GetFailingTypes(TestResult result)
        {
            return result.FailingTypeNames != null ?
                string.Join(", ", result.FailingTypeNames) :
                string.Empty;
        }
    }
}