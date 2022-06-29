using MediatR;

using RPSSL.Application.Game;

namespace RPSSL.Application.Queries.GetChoice;

public sealed class GetChoiceQuery : IRequest<Choice>
{
}
