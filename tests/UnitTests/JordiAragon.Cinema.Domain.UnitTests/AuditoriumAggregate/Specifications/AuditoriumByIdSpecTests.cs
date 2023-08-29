namespace JordiAragon.Cinema.Domain.UnitTests.AuditoriumAggregate.Specifications
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate;
    using JordiAragon.Cinema.Domain.AuditoriumAggregate.Specifications;
    using JordiAragon.Cinema.Domain.UnitTests.Features.Auditorium.TestUtils;
    using JordiAragon.Cinema.Domain.UnitTests.TestUtils.Constants;
    using Xunit;

    public class AuditoriumByIdSpecTests
    {
        [Fact]
        public void FindAuditoriumByIdSpec_WhenHavingAValidAuditoriumId_ShouldContainTheAuditorium()
        {
            // Arrange
            var id = AuditoriumId.Create(Guid.NewGuid());

            var auditorium1 = Auditorium.Create(id, Constants.Auditorium.Rows, Constants.Auditorium.SeatsPerRow);
            var auditorium2 = CreateAuditoriumUtils.Create();

            var auditoriums = new List<Auditorium>() { auditorium1, auditorium2 };

            var specification = new AuditoriumByIdSpec(id);

            // Act
            var evaluatedList = specification.Evaluate(auditoriums);

            // Assert
            evaluatedList.Should()
                         .ContainSingle(c => c == auditorium1)
                         .And.NotContain(c => c == auditorium2);
        }

        [Fact]
        public void FindAuditoriumByIdSpec_WhenHavingAnInvalidAuditoriumId_ShouldNotContainTheAuditorium()
        {
            // Arrange
            var auditorium1 = CreateAuditoriumUtils.Create();

            var auditoriums = new List<Auditorium>() { auditorium1 };

            var specification = new AuditoriumByIdSpec(AuditoriumId.Create(Guid.NewGuid()));

            // Act
            var evaluatedList = specification.Evaluate(auditoriums);

            // Assert
            evaluatedList.Should().BeEmpty();
            evaluatedList.Should()
                         .NotContain(c => c == auditorium1);
        }

        [Fact]
        public void FindAuditoriumByIdSpec_WhenHavingANullAuditoriumId_ShouldThrowArgumentNullException()
        {
            // Arrange
            AuditoriumId auditoriumId = null;

            // Act
            Func<AuditoriumByIdSpec> auditorium = () => new AuditoriumByIdSpec(auditoriumId);

            // Assert
            auditorium.Should().Throw<ArgumentNullException>();
        }
    }
}