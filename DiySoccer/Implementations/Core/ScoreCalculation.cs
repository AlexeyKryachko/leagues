using System.Collections.Generic;
using System.Linq;
using Interfaces.Games.DataAccess.Model;

namespace Implementations.Core
{
    public class ScoreCalculation
    {
        public int Points(IEnumerable<GameDb> games, string teamId)
        {
            var wins = WinsCount(games, teamId);
            var loses = LosesCount(games, teamId);
            var drafts = DraftCount(games, teamId);
            return Default(wins, loses, drafts);
        }

        public int DraftCount(IEnumerable<GameDb> games, string teamId)
        {
            return games.Count(x =>
                (x.GuestTeam.Id == teamId && x.GuestTeam.Score == x.HomeTeam.Score) ||
                (x.HomeTeam.Id == teamId && x.HomeTeam.Score == x.GuestTeam.Score));
        }

        public int LosesCount(IEnumerable<GameDb> games, string teamId)
        {
            return games.Count(x =>
                (x.GuestTeam.Id == teamId && x.GuestTeam.Score < x.HomeTeam.Score) ||
                (x.HomeTeam.Id == teamId && x.HomeTeam.Score < x.GuestTeam.Score));
        }

        public int WinsCount(IEnumerable<GameDb> games, string teamId)
        {
            return games.Count(x =>
                (x.GuestTeam.Id == teamId && x.GuestTeam.Score > x.HomeTeam.Score) ||
                (x.HomeTeam.Id == teamId && x.HomeTeam.Score > x.GuestTeam.Score));
        }

        public int GameCount(IEnumerable<GameDb> games, string teamId)
        {
            return games.Count(x => x.GuestTeam.Id == teamId || x.HomeTeam.Id == teamId);
        }

        public int Default(int wins, int loses, int draws)
        {
            return wins*3 + draws;
        }
    }
}
