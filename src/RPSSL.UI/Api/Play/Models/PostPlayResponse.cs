using System.ComponentModel.DataAnnotations;

namespace RPSSL.UI.Api.Play.Models;

public sealed class PostPlayResponse
{
    /// <summary>
    ///     The choice id selected by the player.
    /// </summary>
    [Required]
    public int Player { get; set; }

    /// <summary>
    ///     The choice id selected by the computer.
    /// </summary>
    [Required]
    public int Computer { get; set; }

    /// <summary>
    ///     The result of the game.
    /// </summary>
    [Required]
    public string Results { get; set; }
}
