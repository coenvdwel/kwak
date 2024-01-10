using Kwak.Game;

namespace Kwak.Strategies;

public static class SpendDiamonds
{
  public static int Always(Player p)
  {
    return p.Diamonds / 2;
  }
}