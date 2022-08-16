using static RPSSL.Application.Constants.Game.Choice;

namespace RPSSL.Application.Game;

public sealed class Choice
{
    public static readonly Choice Rock = new(RockId, RockName);
    public static readonly Choice Paper = new(PaperId, PaperName);
    public static readonly Choice Scissors = new(ScissorsId, ScissorsName);
    public static readonly Choice Spock = new(SpockId, SpockName);
    public static readonly Choice Lizard = new(LizardId, LizardName);

    public Choice(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }

    public string Name { get; }
}
