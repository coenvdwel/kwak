﻿using Kwak.Game;
using Kwak.Utility;

namespace Kwak.Strategies;

public static class SpendMoney
{
  public static void Random(Purchase p)
  {
    var tokens = Token.Get
      .Where(x => x.Key != TokenColor.White)
      .SelectMany(x => x.Value.Values.Where(y => y.Cost <= p.Money))
      .ToList();

    while (p.Money >= 3 && tokens.Count > 0)
    {
      var token = tokens[Kwak.Random.Next(tokens.Count)];

      p.Buy(token.TokenColor, token.Value);

      tokens.RemoveAll(x => x.TokenColor == token.TokenColor);
      tokens.RemoveAll(x => x.Cost > p.Money);
    }
  }

  public static void PurpleAndYellow(Purchase p)
  {
    // get our black token first
    p.Buy(TokenColor.Black, 1, max: 1);

    // then, get our 3 purples
    p.Buy(TokenColor.Purple, 1, max: 3);

    // then we prefer buying 2 tokens if we can, so only buy these if you can buy at least 1 other token after
    p.Buy(TokenColor.Yellow, 4, withSecondBuy: true);
    p.Buy(TokenColor.Yellow, 2, withSecondBuy: true);
    p.Buy(TokenColor.Yellow, 1, withSecondBuy: true);

    // if anything left, get these
    p.Buy(TokenColor.Blue, 2);
    p.Buy(TokenColor.Blue, 1);
    p.Buy(TokenColor.Green, 1);
    p.Buy(TokenColor.Orange, 1);
  }

  public static void PurpleAndBlue(Purchase p)
  {
    // get our black token first
    p.Buy(TokenColor.Black, 1, max: 1);

    // then our 3 purples
    p.Buy(TokenColor.Purple, 1, max: 3);

    // then attempt to get best blues
    p.Buy(TokenColor.Blue, 4);
    p.Buy(TokenColor.Blue, 2);
    p.Buy(TokenColor.Blue, 1);

    // if anything left, get these
    p.Buy(TokenColor.Green, 1);
    p.Buy(TokenColor.Orange, 1);
  }

  public static void Blue(Purchase p)
  {
    // get our black token first
    p.Buy(TokenColor.Black, 1, max: 1);

    // then attempt to get best blues
    p.Buy(TokenColor.Blue, 4);
    p.Buy(TokenColor.Blue, 2);
    p.Buy(TokenColor.Blue, 1);

    // if anything left, get these
    p.Buy(TokenColor.Green, 1);
    p.Buy(TokenColor.Orange, 1);
  }

  public static void RedAndOrange(Purchase p)
  {
    // get our black token first
    p.Buy(TokenColor.Black, 1, max: 1);

    // first we prefer buying 2 tokens if we can, so only buy these if you can buy at least 1 other token after
    p.Buy(TokenColor.Red, 4, withSecondBuy: true);
    p.Buy(TokenColor.Red, 2, withSecondBuy: true);

    // if anything left, get these
    p.Buy(TokenColor.Red, 1);
    p.Buy(TokenColor.Orange, 1);
  }

  public static void BigTokens(Purchase p)
  {
    // get our black token first
    p.Buy(TokenColor.Black, 1, max: 1);

    // then our 3 purples
    p.Buy(TokenColor.Purple, 1, max: 3);

    // then the biggest tokens we can buy
    p.Buy(TokenColor.Blue, 4); // 19
    p.Buy(TokenColor.Yellow, 4); // 18
    p.Buy(TokenColor.Red, 4); // 16
    p.Buy(TokenColor.Green, 4); // 14
    p.Buy(TokenColor.Yellow, 2); // 12
    p.Buy(TokenColor.Blue, 2); // 10
    p.Buy(TokenColor.Red, 2); // 10
    p.Buy(TokenColor.Green, 2); // 8
    p.Buy(TokenColor.Yellow, 1); // 8
    p.Buy(TokenColor.Red, 1); // 6
    p.Buy(TokenColor.Blue, 1); // 5
    p.Buy(TokenColor.Green, 1); // 4
    p.Buy(TokenColor.Orange, 1); // 3
  }
}