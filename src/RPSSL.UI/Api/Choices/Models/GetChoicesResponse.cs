using System.ComponentModel.DataAnnotations;

namespace RPSSL.UI.Api.Choices.Models;

public sealed class GetChoicesResponse
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
