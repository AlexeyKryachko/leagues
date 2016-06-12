using System;
using System.Collections.Generic;
using System.Linq;
using Implementations.Games;
using Interfaces.Core;
using Interfaces.Events.DataAccess.Model;
using Interfaces.Games.DataAccess.Model;
using Interfaces.Leagues.BuisnessLogic.Model;
using Interfaces.Teams.BuisnessLogic.Models;
using Interfaces.Teams.DataAccess;
using Interfaces.Users.DataAccess;

namespace Implementations.Teams
{
    public class TeamsMapper
    {
        private readonly ScoreCalculation _scoreCalculation = new ScoreCalculation();

        private IEnumerable<TeamInfoGameViewModel> MapTeamInfoGames(List<GameDb> games, Dictionary<string, TeamDb> teams, List<EventDb> events)
        {
            var result = new List<Tuple<DateTime?, TeamInfoGameViewModel>>();

            foreach (var game in games)
            {
                var model = new TeamInfoGameViewModel
                {
                    Id = game.EntityId,
                    Name = teams[game.HomeTeam.Id].Name + " - " + teams[game.GuestTeam.Id].Name,
                    Goals = game.HomeTeam.Score + " : " + game.GuestTeam.Score
                };

                if (string.IsNullOrEmpty(game.EventId))
                {
                    result.Add(new Tuple<DateTime?, TeamInfoGameViewModel>(null, model));
                    continue;
                }

                var ev = events.FirstOrDefault(x => x.EntityId == game.EventId);
                if (ev == null || !ev.StartDate.HasValue)
                {
                    result.Add(new Tuple<DateTime?, TeamInfoGameViewModel>(null, model));
                    continue;
                }

                model.Event = ev.Name;
                result.Add(new Tuple<DateTime?, TeamInfoGameViewModel>(ev.StartDate, model));
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
                Games = MapTeamInfoGames(games, teams, events),
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
                    homeTeams.Count(x => x.HomeTeam.BestMemberId == user.EntityId);

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

        public TeamStatisticViewModel MapTeamStatistic(TeamDb team, IEnumerable<GameDb> games)
        {
            var model = new TeamStatisticViewModel
            {
                Id = team.EntityId,
                Name = team.Name,
                MediaId = team.MediaId
            };

            model.GamesCount = games.Count(x => x.GuestTeam.Id == team.EntityId || x.HomeTeam.Id == team.EntityId);

            model.Wins = games.Count(x =>
                (x.GuestTeam.Id == team.EntityId && x.GuestTeam.Score > x.HomeTeam.Score) ||
                (x.HomeTeam.Id == team.EntityId && x.HomeTeam.Score > x.GuestTeam.Score));

            model.Loses = games.Count(x =>
                (x.GuestTeam.Id == team.EntityId && x.GuestTeam.Score < x.HomeTeam.Score) ||
                (x.HomeTeam.Id == team.EntityId && x.HomeTeam.Score < x.GuestTeam.Score));

            model.Draws = games.Count(x =>
                (x.GuestTeam.Id == team.EntityId && x.GuestTeam.Score == x.HomeTeam.Score) ||
                (x.HomeTeam.Id == team.EntityId && x.HomeTeam.Score == x.GuestTeam.Score));

            model.Scores = games
                .Where(x => x.GuestTeam.Id == team.EntityId)
                .Sum(x => x.GuestTeam.Score);
            model.Scores += games
                .Where(x => x.HomeTeam.Id == team.EntityId)
                .Sum(x => x.HomeTeam.Score);

            model.Missed = games
                .Where(x => x.GuestTeam.Id == team.EntityId)
                .Sum(x => x.HomeTeam.Score);
            model.Missed += games
                .Where(x => x.HomeTeam.Id == team.EntityId)
                .Sum(x => x.GuestTeam.Score);

            model.Points = _scoreCalculation.Default(model.Wins, model.Loses, model.Draws);

            return model;
        }
    }
}
