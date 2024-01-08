// won't do; flask
// won't do; cards
// todo; introduce yellow/purple at correct round?
// todo; cheat checks?

namespace Kwak;

class Program
{
  static void Main()
  {
    var numberOfGames = 100_000;
    var results = new Dictionary<string, Result>();

    var players = new List<Player>
    {
      new Player("Coen")
      {
        KeepDrawing = Strategies.KeepDrawing.AtLeast80PercentChanceToLive,
        Blue = Strategies.Blue.HighestNonWhite,
        WhenExploded = Strategies.WhenExploded.AlwaysBuy,
        Buy = Strategies.Buy.PurpleThenDoublesPreferYellow,
        Drops = Strategies.Drops.Always
      },
      new Player("Kees")
      {
        KeepDrawing = Strategies.KeepDrawing.NeverDie,
        Blue = Strategies.Blue.HighestNonWhite,
        WhenExploded = Strategies.WhenExploded.AlwaysBuy,
        Buy = Strategies.Buy.PurpleThenDoublesPreferYellow,
        Drops = Strategies.Drops.Always
      }
    };

    for (var i = 0; i < numberOfGames; i++)
    {
      var game = new Game(players);

      game.Play();
      game.Process(results);
    }

    foreach (var (player, result) in results)
    {
      Console.WriteLine(player);
      Console.WriteLine($"Wins: {result.Wins}");
      Console.WriteLine($"Losses: {result.Losses}");
      Console.WriteLine($"Games: {result.Games}");
      Console.WriteLine($"MaximumScore: {result.MaximumScore}");
      Console.WriteLine($"TotalScore: {result.TotalScore}");
      Console.WriteLine($"AverageScore: {result.TotalScore / result.Games}");
      Console.WriteLine($"WinRate: {result.Wins / (decimal) result.Games}");
    }
  }


  public static class Strategies
  {
    public static class KeepDrawing
    {
      // don't draw if your chances of survival are below 20%
      public static bool AtLeast80PercentChanceToLive(Player p)
      {
        var pointsLeft = 7 - p.Board.Whites;

        if (pointsLeft >= 3) return true;

        var badDraws = p.Bag.Where(x => x.Color == Color.White && x.Value > pointsLeft).Count();
        var badDrawOdds = badDraws / (decimal) p.Bag.Count;

        if (badDrawOdds > 0.2m) return false;

        return true;
      }

      // don't draw if there's even a teeny tiny chance to die
      public static bool NeverDie(Player p)
      {
        var pointsLeft = 7 - p.Board.Whites;

        if (pointsLeft >= 3) return true;
        if (p.Bag.Any(x => x.Color == Color.White && x.Value > pointsLeft)) return false;

        return true;
      }
    }

    public static class Blue
    {
      // always play the highest non-white
      public static Token HighestNonWhite(Player p, List<Token> tokens)
      {
        return tokens.Where(x => x.Color != Color.White).OrderByDescending(x => x.Value).FirstOrDefault();
      }
    }

    public static class WhenExploded
    {
      // when exploded, always choose to buy, except in the final round where scoring always yields more points
      public static WhenExplodedResult AlwaysBuy(Player p)
      {
        return p.Round == 8 ? WhenExplodedResult.Score : WhenExplodedResult.Buy;
      }
    }

    public static class Buy
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
        if (toBuy.Count < 2 && toSpend >= 9 && !toBuy.Any(x => x.Color == Color.Purple))
        {
          if (allTokens.Count(x => x.Color == Color.Purple) < 5)
          {
            toSpend -= 9;
            toBuy.Add(new Token {Color = Color.Purple, Value = 1});
          }
        }

        // buy a yellow 4 when you can buy something else, too
        if (toBuy.Count < 2 && toSpend >= 21 && !toBuy.Any(x => x.Color == Color.Yellow))
        {
          toSpend -= 18;
          toBuy.Add(new Token {Color = Color.Yellow, Value = 4});
        }

        // buy a yellow 2 when you can buy something else, too
        if (toBuy.Count < 2 && toSpend >= 15 && !toBuy.Any(x => x.Color == Color.Yellow))
        {
          toSpend -= 12;
          toBuy.Add(new Token {Color = Color.Yellow, Value = 2});
        }

        // buy a yellow 1 when you can buy something else, too
        if (toBuy.Count < 2 && toSpend >= 11 && !toBuy.Any(x => x.Color == Color.Yellow))
        {
          toSpend -= 8;
          toBuy.Add(new Token {Color = Color.Yellow, Value = 1});
        }

        // buy a blue 2 if you can
        if (toBuy.Count < 2 && toSpend >= 10 && !toBuy.Any(x => x.Color == Color.Blue))
        {
          toSpend -= 10;
          toBuy.Add(new Token {Color = Color.Blue, Value = 2});
        }

        // buy a blue 1 if you can
        if (toBuy.Count < 2 && toSpend >= 5 && !toBuy.Any(x => x.Color == Color.Blue))
        {
          toSpend -= 5;
          toBuy.Add(new Token {Color = Color.Blue, Value = 1});
        }

        // buy a green 1 if you can
        if (toBuy.Count < 2 && toSpend >= 4 && !toBuy.Any(x => x.Color == Color.Green))
        {
          toSpend -= 4;
          toBuy.Add(new Token {Color = Color.Green, Value = 1});
        }

        // buy an orange 1 if you can
        if (toBuy.Count < 2 && toSpend >= 3 && !toBuy.Any(x => x.Color == Color.Orange))
        {
          toSpend -= 3;
          toBuy.Add(new Token {Color = Color.Orange, Value = 1});
        }

        return toBuy;
      }

      public static List<Token> RedOrange(Player p)
      {
        var allTokens = new List<Token>();
        var toBuy = new List<Token>();
        var toSpend = p.Board.Money;

        // buy a red 4 when you can
        if (toBuy.Count < 2 && toSpend >= 16 && !toBuy.Any(x => x.Color == Color.Red))
        {
          toSpend -= 16;
          toBuy.Add(new Token {Color = Color.Red, Value = 4});
        }

        // buy a red 2 when you can
        if (toBuy.Count < 2 && toSpend >= 10 && !toBuy.Any(x => x.Color == Color.Red))
        {
          toSpend -= 10;
          toBuy.Add(new Token {Color = Color.Red, Value = 2});
        }

        // buy a red 2 when you can
        if (toBuy.Count < 2 && toSpend >= 6 && !toBuy.Any(x => x.Color == Color.Red))
        {
          toSpend -= 6;
          toBuy.Add(new Token {Color = Color.Red, Value = 1});
        }

        // buy an orange 1 if you can
        if (toBuy.Count < 2 && toSpend >= 3 && !toBuy.Any(x => x.Color == Color.Orange))
        {
          toSpend -= 3;
          toBuy.Add(new Token {Color = Color.Orange, Value = 1});
        }

        return toBuy;
      }
    }

    public static class Drops
    {
      // always buy drops when you can, as much as you can
      public static int Always(Player p)
      {
        return p.Diamonds / 2;
      }
    }
  }

  public class Game
  {
    public List<Player> Players { get; init; }
    public Random Rnd { get; init; }

    public Game(List<Player> players)
    {
      Players = players;
      Rnd = new Random(Guid.NewGuid().GetHashCode());
    }

    public void Play()
    {
      // play 9 rounds

      for (var round = 0; round < 9; round++)
      {
        Log($"Round {round + 1} =================");

        // init round

        var mostPoints = Players.Max(x => x.Score);

        foreach (var player in Players)
        {
          player.Game = this;
          player.Round = round;
          player.Done = false;

          if (round == 0) player.Reset();
          if (round == 5) player.Bag.Add(new() {Color = Color.White, Value = 1});

          player.Board.Init(mostPoints);
        }

        // then take turns (as though it's the last round)

        var playing = true;

        while (playing)
        {
          playing = false;

          foreach (var player in Players)
          {
            if (player.Bag.Count == 0) continue;
            if (player.Board.Position == 33) continue;
            if (player.Done) continue;

            // when a player indicates they want to stop, do so

            if (!player.KeepDrawing(player))
            {
              Log(
                $"[{player.Name}] Stops at {player.Board.Money} ({player.Board.Score}) with {player.Board.Whites} whites");

              player.Done = true;
              continue;
            }

            // else, take a token and add it to your board

            var token = player.Bag.Take();

            Log($"[{player.Name}] Plays a {token.Color} {token.Value}");

            player.Board.Add(token);

            // if you've exploded, we're also stopping

            if (player.IsExploded)
            {
              Log(
                $"[{player.Name}] Exploded at {player.Board.Money} ({player.Board.Score}) with {player.Board.Whites} whites <-------------------");

              player.Done = true;
              continue;
            }

            playing = true;
          }
        }

        // end-of-round

        var highestPosition = Players.Where(x => !x.IsExploded).Max(x => x?.Board?.Position) ?? 0;

        foreach (var player in Players)
        {
          var canScore = true;
          var canBuy = true;

          if (player.IsExploded)
          {
            var whenExploded = player.WhenExploded(player);

            canScore &= whenExploded == WhenExplodedResult.Score;
            canBuy &= whenExploded == WhenExplodedResult.Buy;
          }

          // throw the dice if applicable

          if (!player.IsExploded && player.Board.Position == highestPosition)
          {
            switch (Rnd.Next(6))
            {
              case 0:
              case 1:

                player.Score += 1;
                Log($"[{player.Name}] Throws the dice for +1 points = {player.Score}");

                break;

              case 2:

                player.Score += 2;
                Log($"[{player.Name}] Throws the dice for +2 points = {player.Score}");

                break;

              case 3:

                player.Diamonds += 1;
                Log($"[{player.Name}] Throws the dice for +1 diamonds = {player.Diamonds}");

                break;

              case 4:

                player.Start += 1;
                Log($"[{player.Name}] Throws the dice for +1 start position = {player.Start}");

                break;

              case 5:

                player.Bag.Add(new() {Color = Color.Orange, Value = 1});
                Log($"[{player.Name}] Throws the dice for an extra Orange 1 token");

                break;
            }
          }

          // then score all played tokens (black, green and purple)

          player.Board.EndOfRound();

          // award diamonds

          if (player.Board.Diamonds > 0)
          {
            player.Diamonds += player.Board.Diamonds;

            Log($"[{player.Name}] Receives +{player.Board.Diamonds} diamond(s) = {player.Diamonds}");
          }

          // award points

          if (canScore)
          {
            player.Score += player.Board.Score;

            Log($"[{player.Name}] Scores +{player.Board.Score} points = {player.Score}");
          }

          // do purchases

          if (canBuy)
          {
            if (round < 8)
            {
              var tokens = player.Buy(player);

              foreach (var token in tokens)
              {
                player.Bag.Add(token);

                Log(
                  $"[{player.Name}] Bought a {token.Color} {token.Value} from this rounds' {player.Board.Money} gold");
              }
            }
            else
            {
              // in the final round, always buy points instead
              player.Score += player.Board.Money / 5;

              Log(
                $"[{player.Name}] Bought {player.Board.Money / 5} points from the final rounds' {player.Board.Money} gold");
            }
          }

          // buy drops

          if (round < 8)
          {
            var drops = player.Drops(player);

            if (drops > 0)
            {
              player.Diamonds -= drops * 2;
              player.Start += drops;

              Log(
                $"[{player.Name}] Bought +{drops} start position = {player.Start} for -{drops * 2} diamonds = {player.Diamonds}");
            }
          }
          else
          {
            // in the final round, always buy points instead
            player.Score += player.Diamonds / 2;

            Log(
              $"[{player.Name}] Bought {player.Diamonds / 2} points from the final rounds' remaining {player.Diamonds} diamonds");
          }
        }
      }
    }

    public void Process(Dictionary<string, Result> results)
    {
      var best = Players.Max(x => x.Score);

      foreach (var player in Players)
      {
        if (!results.ContainsKey(player.Name)) results[player.Name] = new();
        if (player.Score > results[player.Name].MaximumScore) results[player.Name].MaximumScore = player.Score;
        if (player.Score == best) results[player.Name].Wins += 1;
        else results[player.Name].Losses += 1;

        results[player.Name].Games += 1;
        results[player.Name].TotalScore += player.Score;
      }
    }
  }

  public class Player
  {
    public int Start { get; set; }
    public int Score { get; set; }
    public int Diamonds { get; set; }

    public Game Game { get; set; }
    public int Round { get; set; }
    public bool Done { get; set; }

    public string Name { get; init; }
    public Bag Bag { get; init; }
    public Board Board { get; init; }

    public bool IsExploded => Board.Whites > 7;

    public Func<Player, bool> KeepDrawing { get; init; }
    public Func<Player, List<Token>, Token> Blue { get; init; }
    public Func<Player, WhenExplodedResult> WhenExploded { get; init; }
    public Func<Player, List<Token>> Buy { get; init; }
    public Func<Player, int> Drops { get; init; }

    public Player(string name)
    {
      Name = name;
      Bag = new(this);
      Board = new(this);
    }

    public void Reset()
    {
      Start = Score = Diamonds = 0;

      Bag.Clear();
      Board.Tokens.Clear();
      Board.Init(0);

      Bag.Add(new() {Color = Color.White, Value = 1});
      Bag.Add(new() {Color = Color.White, Value = 1});
      Bag.Add(new() {Color = Color.White, Value = 1});
      Bag.Add(new() {Color = Color.White, Value = 1});
      Bag.Add(new() {Color = Color.White, Value = 2});
      Bag.Add(new() {Color = Color.White, Value = 2});
      Bag.Add(new() {Color = Color.White, Value = 3});
      Bag.Add(new() {Color = Color.Orange, Value = 1});
      Bag.Add(new() {Color = Color.Blue, Value = 1});
    }
  }

  public class Bag : List<Token>
  {
    public Player Player { get; init; }
    public Random Rnd { get; init; }

    public Bag(Player player)
    {
      Player = player;
      Rnd = new Random(Guid.NewGuid().GetHashCode());
    }

    public Token Take()
    {
      var i = Rnd.Next(Count - 1);
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

  public class Board
  {
    public int Position { get; private set; }
    public int Whites { get; private set; }

    public int Score => Scores[Position + 1];
    public int Money => Moneys[Position + 1];
    public int Diamonds => Diamon[Position + 1];

    public Player Player { get; init; }
    public List<Token> Tokens { get; init; }

    public static int[] Moneys = new[]
    {
      00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 10, 11, 12, 13, 14, 15, 15, 16, 16, 17, 17, 18, 18, 19, 19, 20, 20, 21,
      21,
      22, 22, 23, 23, 24, 24, 25, 25, 26, 26, 27, 27, 28, 28, 29, 29, 30, 30, 31, 31, 32, 32, 33, 33, 35
    };

    public static int[] Scores = new[]
    {
      00, 00, 00, 00, 00, 00, 01, 01, 01, 01, 02, 02, 02, 02, 03, 03, 03, 03, 04, 04, 04, 04, 05, 05, 05, 05, 06, 06,
      06,
      07, 07, 07, 08, 08, 08, 09, 09, 09, 10, 10, 10, 11, 11, 11, 12, 12, 12, 12, 13, 13, 13, 14, 14, 15
    };

    public static int[] Diamon = new[]
    {
      00, 00, 00, 00, 00, 01, 00, 00, 00, 01, 00, 00, 00, 01, 00, 00, 01, 00, 00, 00, 01, 00, 00, 00, 01, 00, 00, 00,
      01,
      00, 01, 00, 00, 00, 01, 00, 01, 00, 00, 00, 01, 00, 01, 00, 00, 00, 01, 00, 00, 00, 01, 00, 01, 00
    };

    public static HashSet<int> Rats = new()
      {1, 4, 7, 10, 12, 14, 16, 18, 20, 22, 24, 26, 28, 30, 32, 34, 36, 38, 40, 42, 44, 46, 48};

    public Board(Player player)
    {
      Player = player;
      Tokens = new();
    }

    public void Init(int mostPoints)
    {
      Position = Player.Start;
      Whites = 0;
      Player.Bag.AddRange(Tokens);
      Tokens.Clear();

      var rats = 0;
      for (var i = Player.Score; i < mostPoints; i++)
        if (Rats.Contains(i % 50))
          rats += 1;

      if (rats > 0)
      {
        Position += rats;
        Log($"[{Player.Name}] Adding {rats} rats to start at {Position}");
      }
    }

    public void Add(Token token)
    {
      Position += token.Value;
      Tokens.Add(token);

      if (Position > 33) Position = 33;

      switch (token.Color)
      {
        case Color.Orange:
        case Color.Green:
        case Color.Black:
        case Color.Purple:

          break;

        // for white, just increment our explosion counter
        case Color.White:

          Whites += token.Value;

          break;

        // for red, make sure we take 1 extra step in case of 1 or 2 played orange tokens, or 2 extra steps in case of 3 or more played orange tokens
        case Color.Red:

          var oranges = Tokens.Count(x => x.Color == Color.Orange);
          var bonus = oranges > 2 ? 2 : (oranges > 0 ? 1 : 0);

          Position += bonus;

          break;

        // for blue, we can take more tokens from the bag depending on the value of this blue token, and choose to play none or one of those
        case Color.Blue:

          var extraTokens = Player.Bag.Take(token.Value);
          var extraToken = Player.Blue(Player, extraTokens);

          if (extraToken != null)
          {
            Log(
              $"[{Player.Name}] Plays a {extraToken.Color} {extraToken.Value} for their blue token ({string.Join(',', extraTokens.Select(x => $"{x.Color} {x.Value}"))})");

            Add(extraToken);
            extraTokens.Remove(extraToken);
          }
          else
          {
            Log(
              $"[{Player.Name}] Chose to play nothing for their blue token ({string.Join(',', extraTokens.Select(x => $"{x.Color} {x.Value}"))})");
          }

          Player.Bag.AddRange(extraTokens);

          break;

        // for yellow, when played directly after a white, return that white token to the bag
        case Color.Yellow:

          if (Tokens.Count > 1)
          {
            var lastIndex = Tokens.Count - 2;
            var lastToken = Tokens[lastIndex];

            if (lastToken.Color == Color.White)
            {
              Tokens.RemoveAt(lastIndex);
              Player.Bag.Add(lastToken);
              Whites -= lastToken.Value;

              Log($"[{Player.Name}] A {lastToken.Color} {lastToken.Value} was returned to the bag!");
            }
          }

          break;
      }
    }

    public void EndOfRound()
    {
      // in case the last- or second-to-last token is green, obtain an extra diamond
      if ((Tokens.Count > 0 && Tokens[Tokens.Count - 1].Color == Color.Green) ||
          (Tokens.Count > 1 && Tokens[Tokens.Count - 2].Color == Color.Green))
      {
        Player.Diamonds += 1;
        Log($"[{Player.Name}] Green end-of-round bonus: +1 diamond = {Player.Diamonds}");
      }

      // in case we have black tokens, check others and score
      var blackTokens = Tokens.Count(x => x.Color == Color.Black);
      if (blackTokens > 0)
      {
        var playerIndex = Player.Game.Players.IndexOf(Player);

        if (Player.Game.Players.Count == 2)
        {
          var nextPlayer = Player.Game.Players[(playerIndex + 1) % Player.Game.Players.Count];
          var nextPlayerBlackTokens = nextPlayer.Board.Tokens.Count(x => x.Color == Color.Black);

          if (blackTokens >= nextPlayerBlackTokens)
          {
            Player.Start += 1;
            Log($"[{Player.Name}] Black end-of-round bonus: +1 start position = {Player.Start}");
          }
          else if (blackTokens > nextPlayerBlackTokens)
          {
            Player.Start += 1;
            Player.Diamonds += 1;
            Log(
              $"[{Player.Name}] Black end-of-round bonus: +1 start position = {Player.Start} and +1 diamonds = {Player.Diamonds}");
          }
        }
        else
        {
          var nextPlayer = Player.Game.Players[(playerIndex + 1) % Player.Game.Players.Count];
          var nextPlayerBlackTokens = nextPlayer.Board.Tokens.Count(x => x.Color == Color.Black);
          var previousPlayer =
            Player.Game.Players[(playerIndex + Player.Game.Players.Count - 1) % Player.Game.Players.Count];
          var previousPlayerBlackTokens = previousPlayer.Board.Tokens.Count(x => x.Color == Color.Black);

          if (blackTokens > nextPlayerBlackTokens || blackTokens > previousPlayerBlackTokens)
          {
            Player.Start += 1;
            Log($"[{Player.Name}] Black end-of-round bonus: +1 start position = {Player.Start}");
          }
          else if (blackTokens > nextPlayerBlackTokens && blackTokens > previousPlayerBlackTokens)
          {
            Player.Start += 1;
            Player.Diamonds += 1;
            Log(
              $"[{Player.Name}] Black end-of-round bonus: +1 start position = {Player.Start} and +1 diamonds = {Player.Diamonds}");
          }
        }
      }

      // in case we have purple tokens, score those
      var purpleTokens = Tokens.Count(x => x.Color == Color.Purple);
      if (purpleTokens == 1)
      {
        Player.Score += 1;
        Log($"[{Player.Name}] Purple end-of-round bonus (1): +1 point (= {Player.Score})");
      }
      else if (purpleTokens == 2)
      {
        Player.Score += 1;
        Player.Diamonds += 1;
        Log(
          $"[{Player.Name}] Purple end-of-round bonus (2): +1 point (= {Player.Score}) and +1 diamond (= {Player.Diamonds})");
      }
      else if (purpleTokens > 2)
      {
        Player.Score += 2;
        Player.Start += 1;
        Log(
          $"[{Player.Name}] Purple end-of-round bonus (3+): +2 points (= {Player.Score}) and +1 start position (= {Player.Start})");
      }
    }
  }

  public class Token
  {
    public Color Color { get; set; }
    public int Value { get; set; }
  }

  public enum Color
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

  public class Result
  {
    public long Wins { get; set; }
    public long Losses { get; set; }
    public long Games { get; set; }
    public long MaximumScore { get; set; }
    public long TotalScore { get; set; }
  }

  public enum WhenExplodedResult
  {
    Score = 0,
    Buy = 1
  }

  static void Log(string s)
  {
    //Console.WriteLine(s);
  }
}