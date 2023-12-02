namespace JordiAragon.Cinema.Reservation.ArchitectureTests.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FluentAssertions;
    using JordiAragon.SharedKernel.Domain.Entities;
    using NetArchTest.Rules;
    using Xunit;

    public class EntityTests
    {
        private readonly Assembly assembly;

        public EntityTests()
        {
            this.assembly = AssemblyReference.Assembly;
        }

        [Fact]
        public void Entities_Should_OnlyHaveOnePrivateParameterlessConstructor()
        {
            var types = Types
                .InAssembly(this.assembly)
                .That()
                .Inherit(typeof(BaseEntity<>))
                .GetTypes();

            var failingTypes = new List<Type>();
            foreach (var type in types)
            {
                var constructors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
                if (!Array.Exists(constructors, c => c.IsPrivate && c.GetParameters().Length == 0)
                    && constructors.Length == 1)
                {
                    failingTypes.Add(type);
                }
            }

            failingTypes.Should().BeEmpty();
        }

        [Fact]
        public void Entities_Should_HaveOnlyReadOnlyProperties()
        {
            var types = Types
                .InAssembly(this.assembly)
                .That()
                .Inherit(typeof(BaseEntity<>))
                .GetTypes();

            var failingTypes = new List<Type>();
            foreach (var type in types)
            {
                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    if (property.GetSetMethod(true)?.IsPublic ?? false)
                    {
                        failingTypes.Add(type);
                    }
                }
            }

            failingTypes.Distinct().Should().BeEmpty();
        }
    }
}