namespace Kwak.Strategies;

public static partial class Buy
{
  // buy the highest red when possible, and spend any remainder on oranges

  public static List<Token> RedOrange(Player p)
  {
    var toBuy = new List<Token>();
    var toSpend = p.Board.Money;

    // buy a red 4 when you can
    if (toBuy.Count < 2 && toSpend >= 16 && toBuy.All(x => x.TokenColor != TokenColor.Red))
    {
      toSpend -= 16;
      toBuy.Add(new() {TokenColor = TokenColor.Red, Value = 4});
    }

    // buy a red 2 when you can
    if (toBuy.Count < 2 && toSpend >= 10 && toBuy.All(x => x.TokenColor != TokenColor.Red))
    {
      toSpend -= 10;
      toBuy.Add(new() {TokenColor = TokenColor.Red, Value = 2});
    }

    // buy a red 2 when you can
    if (toBuy.Count < 2 && toSpend >= 6 && toBuy.All(x => x.TokenColor != TokenColor.Red))
    {
      toSpend -= 6;
      toBuy.Add(new() {TokenColor = TokenColor.Red, Value = 1});
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