using System;
using System.Collections.Generic;
using System.Linq;
using Implementations.Core;
using Interfaces.Core;
using Interfaces.Events.DataAccess.Model;
using Interfaces.Games.DataAccess.Model;
using Interfaces.Teams.BuisnessLogic.Models;
using Interfaces.Teams.DataAccess;
using Interfaces.Users.DataAccess;

namespace Implementations.Teams
{
    public class TeamsMapper
    {
        private readonly ScoreCalculation _scoreCalculation = new ScoreCalculation();

        public IdNameViewModel MapIdName(TeamDb team)
        {
            return new IdNameViewModel
            {
                Id = team.EntityId,
                Name = team.Name + "(" + team.LeagueId + ")"
            };
        }

        public TeamViewModel Map(TeamDb team, Dictionary<string, string> players)
        {
            var members = team.MemberIds
                .Where(x => players[x] != null)
                .Select(x => new IdNameViewModel
                {
                    Id = x,
                    Name = players[x]
                });

            return new TeamViewModel
            {
                Id = team.EntityId,
                Name = team.Name,
                Hidden = team.Hidden,
                Members = members,
                Media = team.MediaId,
                Description = team.Description
            };
        }

        private IEnumerable<TeamInfoGameViewModel> MapTeamInfoGames(string teamId, List<GameDb> games, Dictionary<string, TeamDb> teams, List<EventDb> events)
        {
            var result = new List<Tuple<DateTime?, TeamInfoGameViewModel>>();

            foreach (var eventDb in events)
            {
                var game = eventDb.Games.FirstOrDefault(x => x.HomeTeamId == teamId || x.GuestTeamId == teamId);
                if (game == null || string.IsNullOrEmpty(game.HomeTeamId) || string.IsNullOrEmpty(game.GuestTeamId))
                    continue;

                var model = new TeamInfoGameViewModel
                {
                    Name = teams[game.HomeTeamId].Name + " - " + teams[game.GuestTeamId].Name,
                };

                var gameDb = games.FirstOrDefault(x => x.EventId == eventDb.EntityId);
                if (gameDb == null || !eventDb.StartDate.HasValue)
                {
                    result.Add(new Tuple<DateTime?, TeamInfoGameViewModel>(null, model));
                    continue;
                }

                model.Id = gameDb.EntityId;
                model.Goals = gameDb.HomeTeam.Score + " : " + gameDb.GuestTeam.Score;
                
                model.Event = eventDb.Name;
                result.Add(new Tuple<DateTime?, TeamInfoGameViewModel>(eventDb.StartDate, model));
            }
            
            return result
                .OrderByDescending(x => x.Item1)
                .Select(x => x.Item2);
        }

        public TeamInfoViewModel MapTeamInfoViewModel(List<GameDb> games, string teamId, Dictionary<string, TeamDb> teams, List<UserDb> users, List<EventDb> events)
        {
            var stats = new List<TeamInfoStatisticViewModel>()
            {
                new TeamInfoStatisticViewModel(MapTeamStatistic(teams[teamId], games), teams[teamId])
            };

            return new TeamInfoViewModel
            {
                Id = teamId,
                Name = teams[teamId].Name,
                MediaId = teams[teamId].MediaId,
                Description = teams[teamId].Description,
                Games = MapTeamInfoGames(teamId, games, teams, events),
                Players = users.Select(y => new TeamInfoMemberViewModel
                {
                    Id = y.EntityId,
                    Name = y.Name
                }),
                Statistics = stats,
                PlayersStatistics = MapPlayersStatistic(teamId, games, users)
            };
        }

        private IEnumerable<TeamInfoPlayersStatisticViewModel> MapPlayersStatistic(string teamId, List<GameDb> games, List<UserDb> users)
        {
            var homeTeams = games.Where(x => x.HomeTeam.Id == teamId).ToList();
            var homeTeamScores = homeTeams.SelectMany(x => x.HomeTeam.Members).ToList();
            var guestTeams = games.Where(x => x.GuestTeam.Id == teamId).ToList();
            var guestTeamScores = guestTeams.SelectMany(x => x.GuestTeam.Members).ToList();

            var result = new List<TeamInfoPlayersStatisticViewModel>();

            for (var index = 0; index < users.Count; index++)
            {
                var user = users[index];

                var goals = homeTeamScores.Where(x => x.Id == user.EntityId).Sum(x => x.Score) +
                    guestTeamScores.Where(x => x.Id == user.EntityId).Sum(x => x.Score);

                var helps = homeTeamScores.Where(x => x.Id == user.EntityId).Sum(x => x.Help) +
                    guestTeamScores.Where(x => x.Id == user.EntityId).Sum(x => x.Help);

                var best = homeTeams.Count(x => x.HomeTeam.BestMemberId == user.EntityId) +
                    guestTeams.Count(x => x.GuestTeam.BestMemberId == user.EntityId);

                result.Add(new TeamInfoPlayersStatisticViewModel
                {
                    Number = "-",
                    Name = user.Name,
                    Best = best,
                    Goal = goals,
                    Help = helps,
                    Sum = goals + helps
                });
            }

            return result.OrderByDescending(x => x.Sum);
        }

        public TeamStatisticsViewModel MapTeamStatistic(TeamDb team, IEnumerable<GameDb> games)
        {
            var model = new TeamStatisticsViewModel
            {
                Id = team.EntityId,
                Name = team.Name,
                MediaId = team.MediaId
            };

            model.GamesCount = _scoreCalculation.GameCount(games, team.EntityId);

            model.Wins = _scoreCalculation.WinsCount(games, team.EntityId);
            model.Loses = _scoreCalculation.LosesCount(games, team.EntityId);
            model.Draws = _scoreCalculation.DraftCount(games, team.EntityId);
            model.Scores = _scoreCalculation.GoalsCount(games, team.EntityId);
            model.Missed = _scoreCalculation.MissedCount(games, team.EntityId);
            model.Points = _scoreCalculation.Default(model.Wins, model.Loses, model.Draws);

            return model;
        }
    }
}
