namespace Kwak;

public class Bag : List<Token>
{
  public Token Take()
  {
    var i = Kwak.Random.Next(Count - 1);
    var token = this[i];

    RemoveAt(i);

    return token;
  }

  public List<Token> Take(int count)
  {
    var realCount = Math.Min(count, Count);
    var result = new List<Token>();

    for (var i = 0; i < realCount; i++) result.Add(Take());

    return result;
  }
}