using System.Collections.Generic;

namespace RPSSL.Application.Game;

public sealed class RuleBook : IRuleBook
{
    private readonly Dictionary<Choice, List<Choice>> _map = new()
    {
        { Choice.Rock, new() { Choice.Scissors, Choice.Lizard } },
        { Choice.Paper, new() { Choice.Rock, Choice.Spock } },
        { Choice.Scissors, new() { Choice.Paper, Choice.Lizard } },
        { Choice.Spock, new() { Choice.Rock, Choice.Scissors } },
        { Choice.Lizard, new() { Choice.Paper, Choice.Spock } }
    };

    public IReadOnlyCollection<Choice> Choices => _map.Keys;

    public Choice Evaluate(Choice first, Choice second)
    {
        if (first == second)
        {
            return null;
        }

        return _map[first].Contains(second)
            ? first
            : second;
    }
}
