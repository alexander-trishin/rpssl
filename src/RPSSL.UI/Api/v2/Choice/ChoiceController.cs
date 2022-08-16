using System.Net.Mime;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace RPSSL.UI.Api.v2.Choice;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
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
    ///     Simply returns "Hello!" message.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult Get(CancellationToken cancellationToken)
    {
        return Ok("Hello!");
    }
}
