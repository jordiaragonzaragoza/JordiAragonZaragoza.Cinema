namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Auditorium.Domain
{
    using System;
    using System.Globalization;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Auditorium.Domain;
    using Xunit;

    public sealed class NameTests
    {
        [Fact]
        public void CreateName_WhenHavingADefaultValue_ShouldThrowArgumentException()
        {
            // Arrange
            string value = default!;

            // Act
            Func<Name> name = () => Name.Create(value);

            // Assert
            name.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CreateName_WhenHavingAEmptyValue_ShouldThrowArgumentException()
        {
            // Arrange
            var value = string.Empty;

            // Act
            Func<Name> name = () => Name.Create(value);

            // Assert
            name.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateName_WhenHavingAWhitespaceValue_ShouldThrowArgumentException()
        {
            // Arrange
            const string value = " ";

            // Act
            Func<Name> name = () => Name.Create(value);

            // Assert
            name.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateName_WhenHavingAValidValue_ShouldReturnName()
        {
            // Arrange
            const string value = "value";

            // Act
            var name = Name.Create(value);

            // Assert
            name.Should().NotBeNull();
        }

        [Fact]
        public void ImplicitConversion_WhenHavingAName_ShouldReturnDateTimeOffset()
        {
            // Arrange
            const string nameValue = "nameValue";
            var name = Name.Create(nameValue);

            // Act
            string result = name;

            // Assert
            result.Should().Be(nameValue);
        }

        [Fact]
        public void ToString_ShouldReturnStringRepresentationOfName()
        {
            // Arrange
            const string value = "value";
            var name = Name.Create(value);

            // Act
            var result = name.ToString();

            // Assert
            result.Should().Be(value.ToString(CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Equality_Checks_ShouldWorkAsExpected()
        {
            // Arrange
            const string value1 = "value1";
            const string value2 = "value2";

            var name1 = Name.Create(value1);
            var name2 = Name.Create(value1);
            var name3 = Name.Create(value2);

            // Act & Assert
            name1.Should().Be(name2);
            name1.Should().NotBe(name3);
        }
    }
}