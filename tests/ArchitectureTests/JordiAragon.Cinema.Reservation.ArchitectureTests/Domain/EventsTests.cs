namespace JordiAragon.Cinema.Reservation.ArchitectureTests.Domain
{
    using System;
    using System.Reflection;
    using FluentAssertions;
    using JordiAragon.SharedKernel.Domain.Events;
    using NetArchTest.Rules;
    using Xunit;

    public sealed class EventsTests
    {
        private readonly Assembly assembly;

        public EventsTests()
        {
            this.assembly = AssemblyReference.Assembly;
        }

        [Fact]
        public void DomainEvents_Should_Be_Sealed()
        {
            // Act.
            var testResult = Types
                .InAssembly(this.assembly)
                .That()
                .ResideInNamespaceContaining("Domain.Events")
                .Should()
                .BeSealed()
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void DomainEvents_Should_Inherit_From_BaseDomainEvent()
        {
            // Act.
            var testResult = Types
                .InAssembly(this.assembly)
                .That()
                .ResideInNamespaceContaining("Domain.Events")
                .Should()
                .Inherit(typeof(BaseDomainEvent))
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void DomainEvents_Should_EndingWith_Event()
        {
            // Act.
            var testResult = Types
                .InAssembly(this.assembly)
                .That()
                .ResideInNamespaceContaining("Domain.Events")
                .Should()
                .HaveNameEndingWith("Event", StringComparison.Ordinal)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }
    }
}