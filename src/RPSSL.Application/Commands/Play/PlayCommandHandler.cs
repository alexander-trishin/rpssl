using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using RPSSL.Application.Game;
using RPSSL.Infrastructure.Services;

namespace RPSSL.Application.Commands.Play;

public sealed class PlayCommandHandler : IRequestHandler<PlayCommand, GameResults>
{
    private readonly IRuleBook _ruleBook;
    private readonly IRandomService _randomService;

    public PlayCommandHandler(IRuleBook ruleBook, IRandomService randomService)
    {
        _ruleBook = ruleBook ?? throw new ArgumentNullException(nameof(ruleBook));
        _randomService = randomService ?? throw new ArgumentNullException(nameof(randomService));
    }

    public async Task<GameResults> Handle(PlayCommand request, CancellationToken cancellationToken)
    {
        var playerChoice = _ruleBook.Choices.GetChoiceById(request.PlayerChoiceId);

        var computer = await _randomService.GetRandomAsync(cancellationToken);
        var computerChoice = _ruleBook.Choices.GetChoiceByWeight(computer.RandomNumber);

        var winner = _ruleBook.Evaluate(playerChoice, computerChoice);

        var result = winner is null
            ? "tie"
            : winner == playerChoice
                ? "win"
                : "lose";

        return new GameResults
        {
            ComputerChoiceId = computerChoice.Id,
            Result = result
        };
    }
}
