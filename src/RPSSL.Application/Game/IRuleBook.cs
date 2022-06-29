using System.Collections.Generic;

namespace RPSSL.Application.Game;

public interface IRuleBook
{
    IReadOnlyCollection<Choice> Choices { get; }

    Choice Evaluate(Choice first, Choice second);
}
