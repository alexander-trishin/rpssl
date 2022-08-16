using MediatR;

using Microsoft.AspNetCore.Mvc;

using RPSSL.Application.Queries.GetChoices;
using RPSSL.UI.Api.Choices;

namespace RPSSL.UI.UnitTests.Api.Choices;

public class ChoicesControllerTests
{
    [Fact]
    public void Constructor_WhenMediatorIsNull_ThenThrowsArgumentNullException()
    {
        // Arrange
        ISender mediator = null;

        // Act
        Func<object> act = () => new ChoicesController(mediator);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>().WithParameterName(nameof(mediator));
    }

    [Fact]
    public async Task Get_WhenRequested_ThenSendsQueryToMediator()
    {
        // Arrange
        var expectedChoices = new[] { new Application.Game.Choice(0, "test") };

        var mediatorMock = new Mock<ISender>();
        mediatorMock
            .Setup(x => x.Send(
                It.IsAny<GetChoicesQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedChoices);

        var controller = new ChoicesController(mediatorMock.Object);

        // Act
        var actual = (OkObjectResult)await controller.Get(CancellationToken.None);

        // Assert
        actual.Value.Should().BeEquivalentTo(expectedChoices);
    }
}
