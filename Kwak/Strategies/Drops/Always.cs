namespace Kwak.Strategies;

public static class Drops
{
  // always buy drops when you can, as much as you can

  public static int Always(Player p)
  {
    return p.Diamonds / 2;
  }
}