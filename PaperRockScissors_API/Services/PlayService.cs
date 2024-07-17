using PaperRockScissors_API.Models;

namespace PaperRockScissors_API.Services
{
    public static class PlayService
    {
        private static Dictionary<Choice, Choice[]> winnigMap = new Dictionary<Choice, Choice[]>
        {
            {Choice.Rock, new[] {Choice.Scissors, Choice.Lizard} },
            {Choice.Paper, new[] {Choice.Rock, Choice.Spock } },
            {Choice.Scissors, new[] {Choice.Paper, Choice.Lizard} },
            {Choice.Lizard, new[] {Choice.Paper, Choice.Spock } },
            {Choice.Spock, new[] {Choice.Rock, Choice.Scissors } },
        };

        public static LinkedList<Game> GameResults = new LinkedList<Game>();

        public static Dictionary<Guid, Game> MultiplayerGames = new Dictionary<Guid, Game>();

        public static Game CreateMultiplayerGame(int numberOfPlayers)
        {
            var newGame = new Game(numberOfPlayers);
            MultiplayerGames.Add(newGame.Id, newGame);
            return newGame;
        }

        public static bool? SetPlayerChoiceForTheGame(Guid gameId, string player, Choice choice)
        {
            if (!MultiplayerGames.ContainsKey(gameId))
                return null;

            var game = MultiplayerGames[gameId];
            return game.AddPlayerChoice(new PlayerChoice(player, choice));
        }

        public static Game? GetGameResult(Guid gameId)
        {
            return MultiplayerGames.GetValueOrDefault(gameId);
        }
    }
}
