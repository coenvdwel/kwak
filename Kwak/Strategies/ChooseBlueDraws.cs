using Kwak.Game;

namespace Kwak.Strategies;

public static class ChooseBlueDraws
{
  public static Token? HighestNonWhite(Player p, List<Token> tokens)
  {
    return tokens.Where(x => x.TokenColor != TokenColor.White).MaxBy(x => x.Value);
  }
}