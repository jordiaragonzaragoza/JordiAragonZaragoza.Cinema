namespace JordiAragonZaragoza.Cinema.Reservation.ArchitectureTests.Domain
{
    using System;
    using System.Reflection;
    using FluentAssertions;
    using JordiAragonZaragoza.SharedKernel.Domain.Events;
    using NetArchTest.Rules;
    using Xunit;

    public sealed class NotificationsTests
    {
        private readonly Assembly assembly;

        public NotificationsTests()
        {
            this.assembly = AssemblyReference.Assembly;
        }

        [Fact]
        public void DomainNotifications_Should_Be_Sealed()
        {
            // Act.
            var testResult = Types
                .InAssembly(this.assembly)
                .That()
                .ResideInNamespaceContaining("Domain.Notifications")
                .Should()
                .BeSealed()
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void DomainNotifications_Should_Inherit_From_BaseDomainEventNotification()
        {
            // Act.
            var testResult = Types
                .InAssembly(this.assembly)
                .That()
                .ResideInNamespaceContaining("Domain.Notifications")
                .Should()
                .Inherit(typeof(BaseDomainEventNotification<>))
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }

        [Fact]
        public void DomainNotifications_Should_EndingWith_Event()
        {
            // Act.
            var testResult = Types
                .InAssembly(this.assembly)
                .That()
                .ResideInNamespaceContaining("Domain.Notifications")
                .Should()
                .HaveNameEndingWith("Notification", StringComparison.Ordinal)
                .GetResult();

            // Assert.
            testResult.IsSuccessful.Should().BeTrue(Utils.GetFailingTypes(testResult));
        }
    }
}