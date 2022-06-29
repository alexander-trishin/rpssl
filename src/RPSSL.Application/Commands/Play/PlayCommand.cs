using MediatR;

namespace RPSSL.Application.Commands.Play;

public sealed class PlayCommand : IRequest<GameResults>
{
    public int PlayerChoiceId { get; init; }
}
