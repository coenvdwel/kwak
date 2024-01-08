namespace Kwak;

public class Board(Player player)
{
  public int Position { get; private set; }
  public int Whites { get; private set; }

  public int Score => Scores[Position + 1];
  public int Money => Moneys[Position + 1];
  public int Diamond => Diamonds[Position + 1];

  public Player Player { get; } = player;
  public List<Token> Tokens { get; } = [];

  public static readonly int[] Moneys = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 15, 16, 16, 17, 17, 18, 18, 19, 19, 20, 20, 21, 21, 22, 22, 23, 23, 24, 24, 25, 25, 26, 26, 27, 27, 28, 28, 29, 29, 30, 30, 31, 31, 32, 32, 33, 33, 35];
  public static readonly int[] Scores = [0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 10, 11, 11, 11, 12, 12, 12, 12, 13, 13, 13, 14, 14, 15];
  public static readonly int[] Diamonds = [0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0];
  public static readonly HashSet<int> Rats = [1, 4, 7, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48];

  public void Init(int mostPoints)
  {
    Position = Player.Start;
    Whites = 0;
    Player.Bag.AddRange(Tokens);
    Tokens.Clear();

    var rats = 0;
    for (var i = Player.Score; i < mostPoints; i++)
    {
      if (Rats.Contains(i % 50))
      {
        Position += 1;
        rats += 1;
      }
    }

    if (rats > 0) Kwak.Log($"[{Player.Name}] Adding {rats} rats to start at {Position}");
  }

  public void Add(Token token)
  {
    Position += token.Value;
    Tokens.Add(token);

    if (Position > 33) Position = 33;

    switch (token.TokenColor)
    {
      // these do nothing when played directly
      case TokenColor.Orange:
      case TokenColor.Green:
      case TokenColor.Black:
      case TokenColor.Purple:

        break;

      // for white, just increment our explosion counter
      case TokenColor.White:

        Whites += token.Value;

        break;

      // for red, make sure we take 1 extra step in case of 1 or 2 played orange tokens, or 2 extra steps in case of 3 or more played orange tokens
      case TokenColor.Red:

        var oranges = Tokens.Count(x => x.TokenColor == TokenColor.Orange);
        var bonus = oranges > 2 ? 2 : (oranges > 0 ? 1 : 0);

        Position += bonus;

        break;

      // for blue, we can take more tokens from the bag depending on the value of this blue token, and choose to play none or one of those
      case TokenColor.Blue:

        var extraTokens = Player.Bag.Take(token.Value);
        var extraToken = Player.Blue(Player, extraTokens);

        if (extraToken != null)
        {
          Kwak.Log($"[{Player.Name}] Plays a {extraToken.TokenColor} {extraToken.Value} for their blue token ({string.Join(',', extraTokens.Select(x => $"{x.TokenColor} {x.Value}"))})");

          Add(extraToken);
          extraTokens.Remove(extraToken);
        }
        else
        {
          Kwak.Log($"[{Player.Name}] Chose to play nothing for their blue token ({string.Join(',', extraTokens.Select(x => $"{x.TokenColor} {x.Value}"))})");
        }

        Player.Bag.AddRange(extraTokens);

        break;

      // for yellow, when played directly after a white, return that white token to the bag
      case TokenColor.Yellow:

        if (Tokens.Count > 1)
        {
          var lastIndex = Tokens.Count - 2;
          var lastToken = Tokens[lastIndex];

          if (lastToken.TokenColor == TokenColor.White)
          {
            Tokens.RemoveAt(lastIndex);
            Player.Bag.Add(lastToken);
            Whites -= lastToken.Value;

            Kwak.Log($"[{Player.Name}] A {lastToken.TokenColor} {lastToken.Value} was returned to the bag!");
          }
        }

        break;

      default:

        throw new ArgumentOutOfRangeException();
    }
  }

  public void EndOfRound()
  {
    // in case the last- or second-to-last token is green, obtain an extra diamond
    if ((Tokens.Count > 0 && Tokens[^1].TokenColor == TokenColor.Green) || (Tokens.Count > 1 && Tokens[^2].TokenColor == TokenColor.Green))
    {
      Player.Diamonds += 1;
      Kwak.Log($"[{Player.Name}] Green end-of-round bonus: +1 diamond = {Player.Diamonds}");
    }

    // in case we have black tokens, check others and score
    var blackTokens = Tokens.Count(x => x.TokenColor == TokenColor.Black);
    if (blackTokens > 0)
    {
      var playerIndex = Player.Game.Players.IndexOf(Player);

      if (Player.Game.Players.Count == 2)
      {
        var nextPlayer = Player.Game.Players[(playerIndex + 1) % Player.Game.Players.Count];
        var nextPlayerBlackTokens = nextPlayer.Board.Tokens.Count(x => x.TokenColor == TokenColor.Black);

        if (blackTokens >= nextPlayerBlackTokens)
        {
          Player.Start += 1;

          Kwak.Log($"[{Player.Name}] Black end-of-round bonus: +1 start position = {Player.Start}");
        }
        else if (blackTokens > nextPlayerBlackTokens)
        {
          Player.Start += 1;
          Player.Diamonds += 1;

          Kwak.Log($"[{Player.Name}] Black end-of-round bonus: +1 start position = {Player.Start} and +1 diamonds = {Player.Diamonds}");
        }
      }
      else
      {
        var nextPlayer = Player.Game.Players[(playerIndex + 1) % Player.Game.Players.Count];
        var nextPlayerBlackTokens = nextPlayer.Board.Tokens.Count(x => x.TokenColor == TokenColor.Black);
        var previousPlayer = Player.Game.Players[(playerIndex + Player.Game.Players.Count - 1) % Player.Game.Players.Count];
        var previousPlayerBlackTokens = previousPlayer.Board.Tokens.Count(x => x.TokenColor == TokenColor.Black);

        if (blackTokens > nextPlayerBlackTokens || blackTokens > previousPlayerBlackTokens)
        {
          Player.Start += 1;

          Kwak.Log($"[{Player.Name}] Black end-of-round bonus: +1 start position = {Player.Start}");
        }
        else if (blackTokens > nextPlayerBlackTokens && blackTokens > previousPlayerBlackTokens)
        {
          Player.Start += 1;
          Player.Diamonds += 1;

          Kwak.Log($"[{Player.Name}] Black end-of-round bonus: +1 start position = {Player.Start} and +1 diamonds = {Player.Diamonds}");
        }
      }
    }

    // in case we have purple tokens, score those
    var purpleTokens = Tokens.Count(x => x.TokenColor == TokenColor.Purple);
    if (purpleTokens == 1)
    {
      Player.Score += 1;

      Kwak.Log($"[{Player.Name}] Purple end-of-round bonus (1): +1 point (= {Player.Score})");
    }
    else if (purpleTokens == 2)
    {
      Player.Score += 1;
      Player.Diamonds += 1;

      Kwak.Log($"[{Player.Name}] Purple end-of-round bonus (2): +1 point (= {Player.Score}) and +1 diamond (= {Player.Diamonds})");
    }
    else if (purpleTokens > 2)
    {
      Player.Score += 2;
      Player.Start += 1;

      Kwak.Log($"[{Player.Name}] Purple end-of-round bonus (3+): +2 points (= {Player.Score}) and +1 start position (= {Player.Start})");
    }
  }
}