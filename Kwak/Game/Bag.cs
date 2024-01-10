namespace Kwak.Game;

public class Bag : List<Token>
{
  public void Setup()
  {
    Clear();
    
    Add(Token.Get[TokenColor.White][1]);
    Add(Token.Get[TokenColor.White][1]);
    Add(Token.Get[TokenColor.White][1]);
    Add(Token.Get[TokenColor.White][1]);
    Add(Token.Get[TokenColor.White][2]);
    Add(Token.Get[TokenColor.White][2]);
    Add(Token.Get[TokenColor.White][3]);
    Add(Token.Get[TokenColor.Orange][1]);
    Add(Token.Get[TokenColor.Green][1]);
  }

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