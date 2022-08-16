using System;
using System.Collections.Generic;
using System.Linq;

namespace RPSSL.Application.Game;

public static class EnumerableExtensions
{
    public static Choice GetChoiceById(this IReadOnlyCollection<Choice> choices, int id)
    {
        if (choices is null)
        {
            throw new ArgumentNullException(nameof(choices));
        }

        return choices.First(choice => choice.Id == id);
    }

    public static Choice GetChoiceByWeight(this IReadOnlyCollection<Choice> choices, int weight)
    {
        if (choices is null)
        {
            throw new ArgumentNullException(nameof(choices));
        }

        const int MaxWeight = 100;
        const int ChoiceCount = 5;

        var id = (int)Math.Ceiling((decimal)weight / (MaxWeight / ChoiceCount));

        return choices.First(choice => choice.Id == id);
    }
}
