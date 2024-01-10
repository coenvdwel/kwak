using Kwak.Game;

namespace Kwak.Strategies;

public static class KeepDrawing
{
  public static bool BetterSafeThanSorry(Player p)
  {
    var pointsLeft = 7 - p.Board.Whites;

    if (pointsLeft >= 3) return true;

    var badDraws = p.Bag.Count(x => x.TokenColor == TokenColor.White && x.Value > pointsLeft);
    var badDrawOdds = badDraws / (decimal) p.Bag.Count;

    if (badDrawOdds > 0.2m) return false;

    return true;
  }
}