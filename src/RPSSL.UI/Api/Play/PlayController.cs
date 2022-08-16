using System.Net.Mime;

using MediatR;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using RPSSL.Application.Commands.Play;
using RPSSL.UI.Api.Play.Models;

namespace RPSSL.UI.Api.Choice;

[ApiController]
[Route("api/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public sealed class PlayController : ControllerBase
{
    private readonly ISender _mediator;

    public PlayController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    ///     Plays a round against a computer opponent.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PostPlayResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(
        [BindRequired][FromBody] PostPlayBody body,
        CancellationToken cancellationToken)
    {
        var command = new PlayCommand { PlayerChoiceId = body.Player.Value };

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(new PostPlayResponse
        {
            Player = command.PlayerChoiceId,
            Computer = result.ComputerChoiceId,
            Results = result.Result
        });
    }
}
