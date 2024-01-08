namespace Kwak.Strategies;

public static partial class Buy
{
  // first buy to up 5 purples, then prioritize buying 2 if you can, focus on yellow

  public static List<Token> PurpleThenDoublesPreferYellow(Player p)
  {
    var allTokens = new List<Token>();
    var toBuy = new List<Token>();
    var toSpend = p.Board.Money;

    allTokens.AddRange(p.Board.Tokens);
    allTokens.AddRange(p.Bag);

    // always buy a purple if you can, but no more than 5 total
    if (toBuy.Count < 2 && toSpend >= 9 && toBuy.All(x => x.TokenColor != TokenColor.Purple))
    {
      if (allTokens.Count(x => x.TokenColor == TokenColor.Purple) < 5)
      {
        toSpend -= 9;
        toBuy.Add(new() {TokenColor = TokenColor.Purple, Value = 1});
      }
    }

    // buy a yellow 4 when you can buy something else, too
    if (toBuy.Count < 2 && toSpend >= 21 && toBuy.All(x => x.TokenColor != TokenColor.Yellow))
    {
      toSpend -= 18;
      toBuy.Add(new() {TokenColor = TokenColor.Yellow, Value = 4});
    }

    // buy a yellow 2 when you can buy something else, too
    if (toBuy.Count < 2 && toSpend >= 15 && toBuy.All(x => x.TokenColor != TokenColor.Yellow))
    {
      toSpend -= 12;
      toBuy.Add(new() {TokenColor = TokenColor.Yellow, Value = 2});
    }

    // buy a yellow 1 when you can buy something else, too
    if (toBuy.Count < 2 && toSpend >= 11 && toBuy.All(x => x.TokenColor != TokenColor.Yellow))
    {
      toSpend -= 8;
      toBuy.Add(new() {TokenColor = TokenColor.Yellow, Value = 1});
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