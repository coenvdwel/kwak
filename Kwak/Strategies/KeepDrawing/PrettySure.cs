namespace Kwak.Strategies;

public static partial class KeepDrawing
{
  // only draw when you're pretty sure you won't die (80% survival)

  public static bool PrettySure(Player p)
  {
    var pointsLeft = 7 - p.Board.Whites;

    if (pointsLeft >= 3) return true;

    var badDraws = p.Bag.Count(x => x.TokenColor == TokenColor.White && x.Value > pointsLeft);
    var badDrawOdds = badDraws / (decimal) p.Bag.Count;

    if (badDrawOdds > 0.2m) return false;

    return true;
  }
}