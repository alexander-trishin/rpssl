namespace RPSSL.Application.Game
{
    public sealed class Choice
    {
        public static readonly Choice Rock = new(1, "rock");
        public static readonly Choice Paper = new(2, "paper");
        public static readonly Choice Scissors = new(3, "scissors");
        public static readonly Choice Spock = new(4, "spock");
        public static readonly Choice Lizard = new(5, "lizard");

        private Choice(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }
    }
}
