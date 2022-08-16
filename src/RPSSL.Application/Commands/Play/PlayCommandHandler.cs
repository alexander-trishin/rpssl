using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using RPSSL.Application.Game;
using RPSSL.Infrastructure.Services;

using static RPSSL.Application.Constants.Game.Result;

namespace RPSSL.Application.Commands.Play;

public sealed class PlayCommandHandler : IRequestHandler<PlayCommand, GameResults>
{
    private readonly IRandomService _randomService;
    private readonly IRuleBook _ruleBook;

    public PlayCommandHandler(IRuleBook ruleBook, IRandomService randomService)
    {
        _ruleBook = ruleBook ?? throw new ArgumentNullException(nameof(ruleBook));
        _randomService = randomService ?? throw new ArgumentNullException(nameof(randomService));
    }

    public async Task<GameResults> Handle(PlayCommand request, CancellationToken cancellationToken)
    {
        var playerChoice = _ruleBook.Choices.GetChoiceById(request.PlayerChoiceId);
        var computerChoice = await GetComputerChoiceAsync(cancellationToken);

        var result = Play(playerChoice, computerChoice);

        return new GameResults
        {
            ComputerChoiceId = computerChoice.Id,
            Result = result
        };
    }

    private async Task<Choice> GetComputerChoiceAsync(CancellationToken cancellationToken)
    {
        var computer = await _randomService.GetRandomAsync(cancellationToken);

        return _ruleBook.Choices.GetChoiceByWeight(computer.RandomNumber);
    }

    private string Play(Choice playerChoice, Choice computerChoice)
    {
        var winner = _ruleBook.Evaluate(playerChoice, computerChoice);

        return winner is null
            ? Tie
            : winner == playerChoice
                ? Win
                : Lose;
    }
}
