using Kwak.Utility;

namespace Kwak.Game;

public class Game(List<Player> players)
{
  public int Round { get; set; }
  public List<Player> Players { get; } = players;

  public void Play()
  {
    Players.ForEach(x => x.Setup(this));

    for (Round = 0; Round < 9; Round++)
    {
      if (Kwak.Log) Console.WriteLine($"Round {Round + 1} =================");

      // init round

      foreach (var player in Players)
      {
        player.Board.Setup();
        player.Board.SetupRats();

        player.Done = false;

        if (Round == 5) player.Bag.Add(Token.Get[TokenColor.White][1]);
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
            if (Kwak.Log) Console.WriteLine($"[{player.Name}] Stops at {player.Board.Money} ({player.Board.Score}) with {player.Board.Whites} whites");

            player.Done = true;
            continue;
          }

          // else, take a token and add it to your board

          var token = player.Bag.Take();

          if (Kwak.Log) Console.WriteLine($"[{player.Name}] Plays a {token.TokenColor} {token.Value}");

          player.Board.Add(token);

          // if you've exploded, we're also stopping

          if (player.IsExploded)
          {
            if (Kwak.Log) Console.WriteLine($"[{player.Name}] Exploded at {player.Board.Money} ({player.Board.Score}) with {player.Board.Whites} whites <-------------------");

            player.Done = true;
            continue;
          }

          playing = true;
        }
      }

      // end-of-round

      var highestPosition = Players.Where(x => !x.IsExploded).Max(x => (int?) x.Board.Position) ?? 0;

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
          switch (Kwak.Random.Next(6))
          {
            case 0:
            case 1:

              player.Score += 1;
              if (Kwak.Log) Console.WriteLine($"[{player.Name}] Throws the dice for +1 points = {player.Score}");

              break;

            case 2:

              player.Score += 2;
              if (Kwak.Log) Console.WriteLine($"[{player.Name}] Throws the dice for +2 points = {player.Score}");

              break;

            case 3:

              player.Diamonds += 1;
              if (Kwak.Log) Console.WriteLine($"[{player.Name}] Throws the dice for +1 diamonds = {player.Diamonds}");

              break;

            case 4:

              player.Start += 1;
              if (Kwak.Log) Console.WriteLine($"[{player.Name}] Throws the dice for +1 start position = {player.Start}");

              break;

            case 5:

              player.Bag.Add(Token.Get[TokenColor.Orange][1]);
              if (Kwak.Log) Console.WriteLine($"[{player.Name}] Throws the dice for an extra Orange 1 token");

              break;
          }
        }

        // then score all played tokens (black, green and purple)

        player.Board.EndOfRound();

        // award diamonds

        if (player.Board.Diamond > 0)
        {
          player.Diamonds += player.Board.Diamond;

          if (Kwak.Log) Console.WriteLine($"[{player.Name}] Receives +{player.Board.Diamond} diamond(s) = {player.Diamonds}");
        }

        // award points

        if (canScore)
        {
          player.Score += player.Board.Score;

          if (Kwak.Log) Console.WriteLine($"[{player.Name}] Scores +{player.Board.Score} points = {player.Score}");
        }

        // do purchases

        if (canBuy)
        {
          if (Round < 8)
          {
            var purchase = new Purchase(player);

            player.SpendMoney(purchase);

            foreach (var token in purchase.Result)
            {
              player.Bag.Add(token);

              if (Kwak.Log) Console.WriteLine($"[{player.Name}] Bought a {token.TokenColor} {token.Value} from this rounds' {player.Board.Money} gold");
            }
          }
          else
          {
            // in the final round, always buy points instead
            player.Score += player.Board.Money / 5;

            if (Kwak.Log) Console.WriteLine($"[{player.Name}] Bought {player.Board.Money / 5} points from the final rounds' {player.Board.Money} gold");
          }
        }

        // buy drops

        if (Round < 8)
        {
          var drops = player.SpendDiamonds(player);

          if (drops > 0)
          {
            player.Diamonds -= drops * 2;
            player.Start += drops;

            if (Kwak.Log) Console.WriteLine($"[{player.Name}] Bought +{drops} start position = {player.Start} for -{drops * 2} diamonds = {player.Diamonds}");
          }
        }
        else
        {
          // in the final round, always buy points instead
          player.Score += player.Diamonds / 2;

          if (Kwak.Log) Console.WriteLine($"[{player.Name}] Bought {player.Diamonds / 2} points from the final rounds' remaining {player.Diamonds} diamonds");
        }
      }
    }

    // scoring

    var best = Players.Max(x => x.Score);

    foreach (var player in Players)
    {
      if (player.Score > player.MaximumScore) player.MaximumScore = player.Score;
      if (player.Score == best) player.Wins += 1;
      else player.Losses += 1;

      player.Games += 1;
      player.TotalScore += player.Score;
    }
  }
}