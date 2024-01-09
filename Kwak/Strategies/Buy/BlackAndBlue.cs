using Kwak.Game;

namespace Kwak.Strategies;

public static partial class Buy
{
  // prefer black and blues

  public static List<Token> BlackAndBlue(Player p)
  {
    var toBuy = new List<Token>();
    var toSpend = p.Board.Money;

    if (toBuy.Count < 2 && toSpend >= 10 && !Utility.HasAtLeast(p, TokenColor.Black, 1))
    {
      toSpend -= 10;
      toBuy.Add(new() {TokenColor = TokenColor.Black, Value = 1});
    }

    // buy a blue 4 if you can
    if (toBuy.Count < 2 && toSpend >= 19 && toBuy.All(x => x.TokenColor != TokenColor.Blue))
    {
      toSpend -= 19;
      toBuy.Add(new() {TokenColor = TokenColor.Blue, Value = 4});
    }

    // buy a blue 2 if you can
    if (toBuy.Count < 2 && toSpend >= 10 && toBuy.All(x => x.TokenColor != TokenColor.Blue))
    {
      toSpend -= 10;
      toBuy.Add(new() {TokenColor = TokenColor.Blue, Value = 2});
    }

    // buy a blue 1 if you can
    if (toBuy.Count < 2 && toSpend >= 5 && toBuy.All(x => x.TokenColor != TokenColor.Blue))
    {
      toSpend -= 5;
      toBuy.Add(new() {TokenColor = TokenColor.Blue, Value = 1});
    }

    // buy a green 1 if you can
    if (toBuy.Count < 2 && toSpend >= 4 && toBuy.All(x => x.TokenColor != TokenColor.Green))
    {
      toSpend -= 4;
      toBuy.Add(new() {TokenColor = TokenColor.Green, Value = 1});
    }

    // buy an orange 1 if you can
    if (toBuy.Count < 2 && toSpend >= 3 && toBuy.All(x => x.TokenColor != TokenColor.Orange))
    {
      toSpend -= 3;
      toBuy.Add(new() {TokenColor = TokenColor.Orange, Value = 1});
    }

    return toBuy;
  }
}