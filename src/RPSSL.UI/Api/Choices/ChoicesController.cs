using System.Net.Mime;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using RPSSL.Application.Queries.GetChoices;
using RPSSL.UI.Api.Choices.Models;

namespace RPSSL.UI.Api.Choices;

[ApiController]
[Route("api/[controller]")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public sealed class ChoicesController : ControllerBase
{
    private readonly ISender _mediator;

    public ChoicesController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Gets the list of all possible choices.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetChoicesResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var results = await _mediator.Send(new GetChoicesQuery(), cancellationToken);

        return Ok(results.Select(result => new GetChoicesResponse
        {
            Id = result.Id,
            Name = result.Name
        }));
    }
}
