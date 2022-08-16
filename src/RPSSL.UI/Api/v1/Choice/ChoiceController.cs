using System.Net.Mime;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using RPSSL.Application.Queries.GetChoice;
using RPSSL.UI.Api.v1.Choice.Models;

namespace RPSSL.UI.Api.v1.Choice;

[ApiController]
[Route("api/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public sealed class ChoiceController : ControllerBase
{
    private readonly ISender _mediator;

    public ChoiceController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    ///     Gets a randomly generated choice.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(GetChoiceResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetChoiceQuery(), cancellationToken);

        return Ok(new GetChoiceResponse
        {
            Id = result.Id,
            Name = result.Name
        });
    }
}
