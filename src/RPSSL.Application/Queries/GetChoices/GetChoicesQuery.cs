using System.Collections.Generic;

using MediatR;

using RPSSL.Application.Game;

namespace RPSSL.Application.Queries.GetChoices;

public sealed class GetChoicesQuery : IRequest<IEnumerable<Choice>>
{
}
