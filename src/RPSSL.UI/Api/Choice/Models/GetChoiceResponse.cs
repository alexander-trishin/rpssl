using System.ComponentModel.DataAnnotations;

namespace RPSSL.UI.Api.Choice.Models;

public sealed class GetChoiceResponse
{
    /// <summary>
    ///     The unique choice id.
    /// </summary>
    [Required]
    public int Id { get; set; }

    /// <summary>
    ///     The name of the choice.
    /// </summary>
    [Required]
    public string Name { get; set; }
}
