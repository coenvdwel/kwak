using Kwak.Game;

namespace Kwak;

// won't do; flask
// won't do; cards
// todo; introduce yellow/purple at correct round?
// todo; cheat checks?

public static class Kwak
{
  public static readonly Random Random = new(Guid.NewGuid().GetHashCode());
  public const bool Log = false;

  public static void Main()
  {
    Console.WriteLine("Start!");

    var numberOfGames = 10_000;
    var results = new Dictionary<string, Result>();

    var players = new List<Player>
    {
      new("Suus")
      {
        KeepDrawing = Strategies.KeepDrawing.PrettySure,
        Blue = Strategies.Blue.HighestNonWhite,
        WhenExploded = Strategies.WhenExploded.AlwaysBuy,
        Buy = Strategies.Buy.PurpleThenDoublesPreferYellow,
        Drops = Strategies.Drops.Always
      },
      new("Kees")
      {
        KeepDrawing = Strategies.KeepDrawing.PrettySure,
        Blue = Strategies.Blue.HighestNonWhite,
        WhenExploded = Strategies.WhenExploded.AlwaysBuy,
        Buy = Strategies.Buy.RedOrange,
        Drops = Strategies.Drops.Always
      },
      new("Coen")
      {
        KeepDrawing = Strategies.KeepDrawing.PrettySure,
        Blue = Strategies.Blue.HighestNonWhite,
        WhenExploded = Strategies.WhenExploded.AlwaysBuy,
        Buy = Strategies.Buy.BlackAndBlue,
        Drops = Strategies.Drops.Always
      }
    };

    for (var i = 0; i < numberOfGames; i++)
    {
      var game = new Game.Game(players);

      game.Play();
      game.Process(results);
    }

    foreach (var (player, result) in results)
    {
      Console.WriteLine(player);
      Console.WriteLine($"- Wins: {result.Wins}");
      Console.WriteLine($"- Losses: {result.Losses}");
      Console.WriteLine($"- Games: {result.Games}");
      Console.WriteLine($"- MaximumScore: {result.MaximumScore}");
      Console.WriteLine($"- TotalScore: {result.TotalScore}");
      Console.WriteLine($"- AverageScore: {result.TotalScore / result.Games}");
      Console.WriteLine($"- WinRate: {result.Wins / (decimal) result.Games}");
    }
  }
}

public class Result
{
  public long Wins { get; set; }
  public long Losses { get; set; }
  public long Games { get; set; }
  public long MaximumScore { get; set; }
  public long TotalScore { get; set; }
}