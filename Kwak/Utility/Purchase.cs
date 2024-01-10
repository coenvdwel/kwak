using Kwak.Game;

namespace Kwak.Utility;

public class Purchase(Player player)
{
  public Player Player { get; } = player;
  public List<Token> Result { get; } = [];
  public int Money { get; private set; } = player.Board.Money;

  public bool Buy(TokenColor color, int value, int? max = null, bool leaveRoomForExtraPurchase = false)
  {
    if (color == TokenColor.Yellow && Player.Game!.Round == 0) return false;
    if (color == TokenColor.Purple && Player.Game!.Round <= 1) return false;
    if (Result.Count == 2) return false;
    if (Result.Any(x => x.TokenColor == color)) return false;

    var token = Token.Get[color][value];

    if (token.Cost + (leaveRoomForExtraPurchase ? 3 : 0) > Money) return false;
    if (max != null && HasAtLeast(color, max.Value)) return false;

    Money -= token.Cost;
    Result.Add(token);

    return true;
  }

  private bool HasAtLeast(TokenColor color, int amount)
  {
    var i = 0;

    foreach (var token in Player.Board.Tokens)
    {
      if (token.TokenColor == color && ++i >= amount) return true;
    }

    foreach (var token in Player.Bag)
    {
      if (token.TokenColor == color && ++i >= amount) return true;
    }

    return false;
  }
}