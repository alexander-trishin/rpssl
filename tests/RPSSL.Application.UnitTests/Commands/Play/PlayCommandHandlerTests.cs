using RPSSL.Application.Commands.Play;
using RPSSL.Application.Game;
using RPSSL.Infrastructure.Services;

using static RPSSL.Application.Constants.Game.Result;

namespace RPSSL.Application.UnitTests.Commands.Play;

public class PlayCommandHandlerTests
{
    [Fact]
    public void Constructor_WhenRuleBookIsNull_ThenThrowsArgumentNullException()
    {
        // Arrange
        var ruleBook = (IRuleBook)null;
        var randomService = Mock.Of<IRandomService>();

        // Act
        Func<object> act = () => new PlayCommandHandler(ruleBook, randomService);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>().WithParameterName(nameof(ruleBook));
    }

    [Fact]
    public void Constructor_WhenRandomServiceIsNull_ThenThrowsArgumentNullException()
    {
        // Arrange
        var ruleBook = Mock.Of<IRuleBook>();
        var randomService = (IRandomService)null;

        // Act
        Func<object> act = () => new PlayCommandHandler(ruleBook, randomService);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>().WithParameterName(nameof(randomService));
    }

    [Fact]
    public async Task Handle_WhenCorrectDataProvided_ThenReturnsGameResults()
    {
        // Arrange
        var command = new PlayCommand { PlayerChoiceId = 3 };
        var expectedGameResults = new GameResults
        {
            ComputerChoiceId = 3,
            Result = Tie
        };

        var ruleBookMock = new Mock<IRuleBook>();
        ruleBookMock
            .SetupGet(x => x.Choices)
            .Returns(new[] { Choice.Scissors });

        var randomServiceMock = new Mock<IRandomService>();
        randomServiceMock
            .Setup(x => x.GetRandomAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetRandomResponse { RandomNumber = 55 });

        var handler = new PlayCommandHandler(ruleBookMock.Object, randomServiceMock.Object);

        // Act
        var actual = await handler.Handle(command, default);

        // Assert
        actual.Should().BeEquivalentTo(expectedGameResults);
    }
}
