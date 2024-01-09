using Kwak.Game;

namespace Kwak;

public static class Utility
{
  public static bool HasAtLeast(Player p, TokenColor color, int amount)
  {
    var i = 0;

    foreach (var token in p.Board.Tokens)
    {
      if (token.TokenColor == color && ++i >= amount) return true;
    }

    foreach (var token in p.Bag)
    {
      if (token.TokenColor == color && ++i >= amount) return true;
    }

    return false;
  }
}