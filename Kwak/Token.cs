namespace Kwak;

public class Token
{
  public TokenColor TokenColor { get; set; }
  public int Value { get; set; }
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