using Kwak.Game;
using Kwak.Strategies;

namespace Kwak;

// won't do; flask
// won't do; cards

public static class Kwak
{
  public const bool Log = false;
  public const int NumberOfGames = 100_000;

  public static readonly Random Random = new(Guid.NewGuid().GetHashCode());

  public static void Main() => Manual();

  // run a manual set of players to debug scenes

  public static void Manual()
  {
    var players = new List<Player>
    {
      new(nameof(SpendMoney.PurpleAndYellow)) {SpendMoney = SpendMoney.PurpleAndYellow},
      new(nameof(SpendMoney.RedAndOrange)) {SpendMoney = SpendMoney.RedAndOrange},
      new(nameof(SpendMoney.PurpleAndBlue)) {SpendMoney = SpendMoney.PurpleAndBlue},
      new(nameof(SpendMoney.BigTokens)) {SpendMoney = SpendMoney.BigTokens}
    };

    for (var i = 0; i < NumberOfGames; i++)
    {
      new Game.Game(players).Play();
    }

    foreach (var player in players.OrderByDescending(x => x.Wins))
    {
      Console.WriteLine(player.ToString());
    }
  }

  // evolve players genetically to find the best strategy

  public static void Evolve(int generations)
  {
    var players = new List<Player> {new("gx-0"), new("gx-1"), new("gx-2"), new("gx-3")};

    for (var g = 0; g < generations; g++)
    {
      // remove the weakest players
      players.Remove(players.OrderBy(x => x.Wins).First());
      players.Remove(players.OrderBy(x => x.Wins).First());

      // generate 2 new, random players
      players.Add(new($"g{g}-0")); // todo
      players.Add(new($"g{g}-1")); // todo

      for (var i = 0; i < NumberOfGames; i++)
      {
        new Game.Game(players).Play();
      }
    }

    foreach (var player in players.OrderByDescending(x => x.Wins))
    {
      Console.WriteLine(player.ToString());
    }
  }
}