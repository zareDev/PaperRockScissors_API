using PaperRockScissors_API.Services;

namespace PaperRockScissors_API.Models
{
    public class Game
    {
        private static Dictionary<Choice, Choice[]> winnigMap = new Dictionary<Choice, Choice[]>
        {
            {Choice.Rock, new[] {Choice.Scissors, Choice.Lizard} },
            {Choice.Paper, new[] {Choice.Rock, Choice.Spock } },
            {Choice.Scissors, new[] {Choice.Paper, Choice.Lizard} },
            {Choice.Lizard, new[] {Choice.Paper, Choice.Spock } },
            {Choice.Spock, new[] {Choice.Rock, Choice.Scissors } },
        };

        private readonly object _lock = new();
        public Guid Id { get; }
        public DateTime Created { get; }
        public int NumberOfPlayers { get; }
        public List<PlayerChoice> PlayersChoices { get; }
        public List<GameResult> Results { get; set; }
        public bool Calculated { get; set; }

        public Game(int numberOfPlayers)
        {
            Id = Guid.NewGuid();
            Created = DateTime.UtcNow;
            NumberOfPlayers = numberOfPlayers;
            PlayersChoices = new List<PlayerChoice>(numberOfPlayers);
            Results = new List<GameResult>();
        }

        public bool AddPlayerChoice(PlayerChoice playerChoice)
        {
            lock (_lock)
            {
                if (PlayersChoices.Count == NumberOfPlayers) return false;

                PlayersChoices.Add(playerChoice);

                if (PlayersChoices.Count == NumberOfPlayers)
                {
                    Play();
                }
            }
            return true;
        }

        private void Play()
        {
            for (int i = 0; i < NumberOfPlayers - 1; i++)
            {
                for (int j = i + 1; j < NumberOfPlayers; j++)
                {
                    var play1 = PlayersChoices[i];
                    var play2 = PlayersChoices[j];
                    var res = GetResult(play1.Choice, play2.Choice);
                    Results.Add(new GameResult(play1, play2, res));
                }
            }

            PlayService.GameResults.AddFirst(this);
        }

        public List<string> GetStringResults()
        {
            return Results.Select(x => $"{x.Player1.Player}({x.Player1.Choice}) vs {x.Player2.Player}({x.Player2.Choice}) | {x.Result}").ToList();
        }

        private static string GetResult(Choice choice1, Choice choice2)
        {
            if (choice1 == choice2) return "tie";

            if (winnigMap[choice1].Contains(choice2)) return "win";

            return "lose";
        }
    }

    public record PlayerChoice(string Player, Choice Choice) { }
    public record GameResult(PlayerChoice Player1, PlayerChoice Player2, string Result) { }
}
