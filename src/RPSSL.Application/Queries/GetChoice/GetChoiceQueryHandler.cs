using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using RPSSL.Application.Game;
using RPSSL.Infrastructure.Services;

namespace RPSSL.Application.Queries.GetChoice;

public sealed class GetChoiceQueryHandler : IRequestHandler<GetChoiceQuery, Choice>
{
    private readonly IRuleBook _ruleBook;
    private readonly IRandomService _randomService;

    public GetChoiceQueryHandler(IRuleBook ruleBook, IRandomService randomService)
    {
        _ruleBook = ruleBook ?? throw new ArgumentNullException(nameof(ruleBook));
        _randomService = randomService ?? throw new ArgumentNullException(nameof(randomService));
    }

    public async Task<Choice> Handle(GetChoiceQuery request, CancellationToken cancellationToken)
    {
        var random = await _randomService.GetRandomAsync(cancellationToken);

        return _ruleBook.Choices.GetChoiceByWeight(random.RandomNumber);
    }
}
