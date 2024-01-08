namespace Kwak;

public class Player
{
  public int Start { get; set; }
  public int Score { get; set; }
  public int Diamonds { get; set; }

  public Game? Game { get; set; }
  public int Round { get; set; }
  public bool Done { get; set; }

  public string Name { get; }
  public Bag Bag { get; }
  public Board Board { get; }

  public bool IsExploded => Board.Whites > 7;

  public required Func<Player, bool> KeepDrawing { get; init; }
  public required Func<Player, List<Token>, Token?> Blue { get; init; }
  public required Func<Player, WhenExplodedResult> WhenExploded { get; init; }
  public required Func<Player, List<Token>> Buy { get; init; }
  public required Func<Player, int> Drops { get; init; }

  public Player(string name)
  {
    Name = name;
    Bag = [];
    Board = new(this);
  }

  public void Reset()
  {
    Start = Score = Diamonds = 0;

    Bag.Clear();
    Board.Tokens.Clear();
    Board.Init(0);

    Bag.Add(new() {TokenColor = TokenColor.White, Value = 1});
    Bag.Add(new() {TokenColor = TokenColor.White, Value = 1});
    Bag.Add(new() {TokenColor = TokenColor.White, Value = 1});
    Bag.Add(new() {TokenColor = TokenColor.White, Value = 1});
    Bag.Add(new() {TokenColor = TokenColor.White, Value = 2});
    Bag.Add(new() {TokenColor = TokenColor.White, Value = 2});
    Bag.Add(new() {TokenColor = TokenColor.White, Value = 3});
    Bag.Add(new() {TokenColor = TokenColor.Orange, Value = 1});
    Bag.Add(new() {TokenColor = TokenColor.Blue, Value = 1});
  }
}

public enum WhenExplodedResult
{
  Score = 0,
  Buy = 1
}