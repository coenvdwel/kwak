using Kwak.Game;

namespace Kwak.Strategies;

public static class WhenExploded
{
  // when exploded, always choose to buy, except in the final round where scoring always yields more points

  public static WhenExplodedResult AlwaysBuy(Player p)
  {
    return p.Round == 8 ? WhenExplodedResult.Score : WhenExplodedResult.Buy;
  }
}