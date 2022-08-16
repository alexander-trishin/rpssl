using MediatR;

using Microsoft.AspNetCore.Mvc;

using RPSSL.Application.Queries.GetChoice;
using RPSSL.UI.Api.Choice;

namespace RPSSL.UI.UnitTests.Api.Choice;

public class ChoiceControllerTests
{
    [Fact]
    public void Constructor_WhenMediatorIsNull_ThenThrowsArgumentNullException()
    {
        // Arrange
        ISender mediator = null;

        // Act
        Func<object> act = () => new ChoiceController(mediator);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>().WithParameterName(nameof(mediator));
    }

    [Fact]
    public async Task Get_WhenRequested_ThenSendsQueryToMediator()
    {
        // Arrange
        var expectedChoice = new Application.Game.Choice(0, "test");

        var mediatorMock = new Mock<ISender>();
        mediatorMock
            .Setup(x => x.Send(
                It.IsAny<GetChoiceQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedChoice);

        var controller = new ChoiceController(mediatorMock.Object);

        // Act
        var actual = (OkObjectResult)await controller.Get(CancellationToken.None);

        // Assert
        actual.Value.Should().BeEquivalentTo(expectedChoice);
    }
}
