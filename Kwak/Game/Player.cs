using Kwak.Utility;

namespace Kwak.Game;

public class Player
{
  // overall variables
  public string Name { get; }
  public long Wins { get; set; }
  public long Losses { get; set; }
  public long Games { get; set; }
  public long MaximumScore { get; set; }
  public long TotalScore { get; set; }

  // behavior
  public Action<Purchase> SpendMoney { get; init; } = Strategies.SpendMoney.Noop;
  public Func<Player, int> SpendDiamonds { get; init; } = Strategies.SpendDiamonds.Always;
  public Func<Player, bool> KeepDrawing { get; init; } = Strategies.KeepDrawing.BetterSafeThanSorry;
  public Func<Player, WhenExplodedResult> WhenExploded { get; init; } = Strategies.WhenExploded.AlwaysBuy;
  public Func<Player, List<Token>, Token?> ChooseBlueDraws { get; init; } = Strategies.ChooseBlueDraws.HighestNonWhite;

  // game variables
  public int Start { get; set; }
  public int Score { get; set; }
  public int Diamonds { get; set; }
  public Bag Bag { get; }
  public Board Board { get; }

  // round variables
  public Game? Game { get; set; }
  public bool Done { get; set; }

  public bool IsExploded => Board.Whites > 7;

  public Player(string name)
  {
    Name = name;
    Bag = [];
    Board = new(this);
  }

  public void Setup(Game game)
  {
    Game = game;
    Start = Score = Diamonds = 0;

    Board.Setup();
    Bag.Setup();
  }

  public override string ToString()
  {
    return $"""
            ---------------------
            # {Name}
            ---------------------
            Results: {Wins} wins ({Math.Round(Wins / (decimal) Games * 100, 2)}%) / {Losses} losses
            Total score: {TotalScore}
            Best score: {MaximumScore}
            Average score: {TotalScore / Games}

            """;
  }
}

public enum WhenExplodedResult
{
  Score = 0,
  Buy = 1
}