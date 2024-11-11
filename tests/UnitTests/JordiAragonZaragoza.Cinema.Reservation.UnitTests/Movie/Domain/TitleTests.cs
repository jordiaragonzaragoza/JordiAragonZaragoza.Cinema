namespace JordiAragonZaragoza.Cinema.Reservation.UnitTests.Movie.Domain
{
    using System;
    using System.Globalization;
    using FluentAssertions;
    using JordiAragonZaragoza.Cinema.Reservation.Movie.Domain;
    using Xunit;

    public sealed class TitleTests
    {
        [Fact]
        public void CreateTitle_WhenHavingADefaultValue_ShouldThrowArgumentException()
        {
            // Arrange
            string value = default!;

            // Act
            Func<Title> title = () => Title.Create(value);

            // Assert
            title.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void CreateTitle_WhenHavingAEmptyValue_ShouldThrowArgumentException()
        {
            // Arrange
            var value = string.Empty;

            // Act
            Func<Title> title = () => Title.Create(value);

            // Assert
            title.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateTitle_WhenHavingAWhitespaceValue_ShouldThrowArgumentException()
        {
            // Arrange
            const string value = " ";

            // Act
            Func<Title> title = () => Title.Create(value);

            // Assert
            title.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void CreateTitle_WhenHavingAValidValue_ShouldReturnTitle()
        {
            // Arrange
            const string value = "value";

            // Act
            var title = Title.Create(value);

            // Assert
            title.Should().NotBeNull();
        }

        [Fact]
        public void ImplicitConversion_WhenHavingATitle_ShouldReturnDateTimeOffset()
        {
            // Arrange
            const string titleValue = "titleValue";
            var title = Title.Create(titleValue);

            // Act
            string result = title;

            // Assert
            result.Should().Be(titleValue);
        }

        [Fact]
        public void ToString_ShouldReturnStringRepresentationOfTitle()
        {
            // Arrange
            const string value = "value";
            var title = Title.Create(value);

            // Act
            var result = title.ToString();

            // Assert
            result.Should().Be(value.ToString(CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Equality_Checks_ShouldWorkAsExpected()
        {
            // Arrange
            const string value1 = "value1";
            const string value2 = "value2";

            var title1 = Title.Create(value1);
            var title2 = Title.Create(value1);
            var title3 = Title.Create(value2);

            // Act & Assert
            title1.Should().Be(title2);
            title1.Should().NotBe(title3);
        }
    }
}