using Kwak.Game;

namespace Kwak.Strategies;

public static partial class KeepDrawing
{
  // don't take a single risk - if there's even a teeny tiny chance of dying, stop drawing

  public static bool NoChances(Player p)
  {
    var pointsLeft = 7 - p.Board.Whites;

    if (pointsLeft >= 3) return true;
    if (p.Bag.Any(x => x.TokenColor == TokenColor.White && x.Value > pointsLeft)) return false;

    return true;
  }
}