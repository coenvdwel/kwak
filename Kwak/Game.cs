namespace Kwak;

public class Game(List<Player> players)
{
  public List<Player> Players { get; } = players;

  public void Play()
  {
    for (var round = 0; round < 9; round++)
    {
      Kwak.Log($"Round {round + 1} =================");

      // init round

      var mostPoints = Players.Max(x => x.Score);

      foreach (var player in Players)
      {
        player.Game = this;
        player.Round = round;
        player.Done = false;

        if (round == 0) player.Reset();
        if (round == 5) player.Bag.Add(new() {TokenColor = TokenColor.White, Value = 1});

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
            Kwak.Log($"[{player.Name}] Stops at {player.Board.Money} ({player.Board.Score}) with {player.Board.Whites} whites");

            player.Done = true;
            continue;
          }

          // else, take a token and add it to your board

          var token = player.Bag.Take();

          Kwak.Log($"[{player.Name}] Plays a {token.TokenColor} {token.Value}");

          player.Board.Add(token);

          // if you've exploded, we're also stopping

          if (player.IsExploded)
          {
            Kwak.Log($"[{player.Name}] Exploded at {player.Board.Money} ({player.Board.Score}) with {player.Board.Whites} whites <-------------------");

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
              Kwak.Log($"[{player.Name}] Throws the dice for +1 points = {player.Score}");

              break;

            case 2:

              player.Score += 2;
              Kwak.Log($"[{player.Name}] Throws the dice for +2 points = {player.Score}");

              break;

            case 3:

              player.Diamonds += 1;
              Kwak.Log($"[{player.Name}] Throws the dice for +1 diamonds = {player.Diamonds}");

              break;

            case 4:

              player.Start += 1;
              Kwak.Log($"[{player.Name}] Throws the dice for +1 start position = {player.Start}");

              break;

            case 5:

              player.Bag.Add(new() {TokenColor = TokenColor.Orange, Value = 1});
              Kwak.Log($"[{player.Name}] Throws the dice for an extra Orange 1 token");

              break;
          }
        }

        // then score all played tokens (black, green and purple)

        player.Board.EndOfRound();

        // award diamonds

        if (player.Board.Diamond > 0)
        {
          player.Diamonds += player.Board.Diamond;

          Kwak.Log($"[{player.Name}] Receives +{player.Board.Diamond} diamond(s) = {player.Diamonds}");
        }

        // award points

        if (canScore)
        {
          player.Score += player.Board.Score;

          Kwak.Log($"[{player.Name}] Scores +{player.Board.Score} points = {player.Score}");
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

              Kwak.Log($"[{player.Name}] Bought a {token.TokenColor} {token.Value} from this rounds' {player.Board.Money} gold");
            }
          }
          else
          {
            // in the final round, always buy points instead
            player.Score += player.Board.Money / 5;

            Kwak.Log($"[{player.Name}] Bought {player.Board.Money / 5} points from the final rounds' {player.Board.Money} gold");
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

            Kwak.Log($"[{player.Name}] Bought +{drops} start position = {player.Start} for -{drops * 2} diamonds = {player.Diamonds}");
          }
        }
        else
        {
          // in the final round, always buy points instead
          player.Score += player.Diamonds / 2;

          Kwak.Log($"[{player.Name}] Bought {player.Diamonds / 2} points from the final rounds' remaining {player.Diamonds} diamonds");
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