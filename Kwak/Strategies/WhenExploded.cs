using Kwak.Game;

namespace Kwak.Strategies;

public static class WhenExploded
{
  public static WhenExplodedResult AlwaysBuy(Player p)
  {
    // in the final round, when exploded, scoring always yields more than buying
    return p.Game!.Round == 8 ? WhenExplodedResult.Score : WhenExplodedResult.Buy;
  }
}