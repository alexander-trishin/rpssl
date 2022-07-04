namespace RPSSL.Application;

public static class Constants
{
    public static class Game
    {
        public static class Choice
        {
            public const int RockId = 1;
            public const string RockName = "rock";

            public const int PaperId = 2;
            public const string PaperName = "paper";

            public const int ScissorsId = 3;
            public const string ScissorsName = "scissors";

            public const int SpockId = 4;
            public const string SpockName = "spock";

            public const int LizardId = 5;
            public const string LizardName = "lizard";
        }

        public static class Result
        {
            public const string Tie = "tie";
            public const string Win = "win";
            public const string Lose = "lose";
        }
    }
}
