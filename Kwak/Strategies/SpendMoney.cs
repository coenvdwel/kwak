using Kwak.Game;
using Kwak.Utility;

namespace Kwak.Strategies;

public static class SpendMoney
{
  public static void Noop(Purchase p)
  {
  }

  public static void PurpleAndYellow(Purchase p)
  {
    // get our 3 purple(s) first
    p.Buy(TokenColor.Purple, 1, max: 3);

    // then we prefer buying 2 tokens if we can, so only buy these if you can buy at least 1 other token after
    p.Buy(TokenColor.Yellow, 4, leaveRoomForExtraPurchase: true);
    p.Buy(TokenColor.Yellow, 2, leaveRoomForExtraPurchase: true);
    p.Buy(TokenColor.Yellow, 1, leaveRoomForExtraPurchase: true);

    // if anything left, get these
    p.Buy(TokenColor.Blue, 2);
    p.Buy(TokenColor.Blue, 1);
    p.Buy(TokenColor.Green, 1);
    p.Buy(TokenColor.Orange, 1);
  }

  public static void PurpleAndBlue(Purchase p)
  {
    // get our 3 purple(s) first
    p.Buy(TokenColor.Purple, 1, max: 3);

    // then attempt to get best blues
    p.Buy(TokenColor.Blue, 4);
    p.Buy(TokenColor.Blue, 2);
    p.Buy(TokenColor.Blue, 1);

    // if anything left, get these
    p.Buy(TokenColor.Green, 1);
    p.Buy(TokenColor.Orange, 1);
  }

  public static void BlackAndBlue(Purchase p)
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
    // first we prefer buying 2 tokens if we can, so only buy these if you can buy at least 1 other token after
    p.Buy(TokenColor.Red, 4, leaveRoomForExtraPurchase: true);
    p.Buy(TokenColor.Red, 2, leaveRoomForExtraPurchase: true);

    // if anything left, get these
    p.Buy(TokenColor.Red, 1);
    p.Buy(TokenColor.Orange, 1);
  }
}