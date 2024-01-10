namespace Kwak.Game;

public class Token(TokenColor color, int value, int cost)
{
  public TokenColor TokenColor { get; set; } = color;
  public int Value { get; set; } = value;
  public int Cost { get; set; } = cost;

  public static readonly Dictionary<TokenColor, Dictionary<int, Token>> Get = new()
  {
    {
      TokenColor.White, new()
      {
        {1, new(TokenColor.White, 1, 0)},
        {2, new(TokenColor.White, 2, 0)},
        {3, new(TokenColor.White, 3, 0)},
      }
    },
    {
      TokenColor.Orange, new()
      {
        {1, new(TokenColor.Orange, 1, 3)}
      }
    },
    {
      TokenColor.Green, new()
      {
        {1, new(TokenColor.Green, 1, 4)},
        {2, new(TokenColor.Green, 2, 8)},
        {4, new(TokenColor.Green, 4, 14)}
      }
    },
    {
      TokenColor.Red, new()
      {
        {1, new(TokenColor.Red, 1, 6)},
        {2, new(TokenColor.Red, 2, 10)},
        {4, new(TokenColor.Red, 4, 16)}
      }
    },
    {
      TokenColor.Blue, new()
      {
        {1, new(TokenColor.Blue, 1, 5)},
        {2, new(TokenColor.Blue, 2, 10)},
        {4, new(TokenColor.Blue, 4, 19)}
      }
    },
    {
      TokenColor.Black, new()
      {
        {1, new(TokenColor.Black, 1, 10)}
      }
    },
    {
      TokenColor.Yellow, new()
      {
        {1, new(TokenColor.Yellow, 1, 8)},
        {2, new(TokenColor.Yellow, 2, 12)},
        {4, new(TokenColor.Yellow, 4, 18)}
      }
    },
    {
      TokenColor.Purple, new()
      {
        {1, new(TokenColor.Purple, 1, 9)}
      }
    }
  };
}

public enum TokenColor
{
  White = 0,
  Orange = 1,
  Green = 2,
  Red = 3,
  Blue = 4,
  Black = 5,
  Yellow = 6,
  Purple = 7
}