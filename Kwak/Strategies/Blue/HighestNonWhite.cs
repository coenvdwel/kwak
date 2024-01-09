using Kwak.Game;

namespace Kwak.Strategies;

public static class Blue
{
  // always play the highest non-white

  public static Token? HighestNonWhite(Player p, List<Token> tokens)
  {
    return tokens.Where(x => x.TokenColor != TokenColor.White).MaxBy(x => x.Value);
  }
}