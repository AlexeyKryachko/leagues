using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;
using Interfaces.Games.DataAccess;
using Interfaces.Leagues.BuisnessLogic;
using Interfaces.Leagues.BuisnessLogic.Model;
using Interfaces.Leagues.DataAccess;
using Interfaces.Teams.BuisnessLogic.Models;
using Interfaces.Teams.DataAccess;
using Interfaces.Users.DataAccess;

namespace Implementations.Leagues.BuisnessLogic
{
    public class LeaguesManager : ILeaguesManager
    {
        private readonly ILeaguesRepository _leaguesRepository;
        private readonly ITeamsRepository _teamsRepository;
        private readonly IGamesRepository _gamesRepository;
        private readonly IUsersRepository _usersRepository;

        public LeaguesManager(ILeaguesRepository leaguesRepository, ITeamsRepository teamsRepository, IGamesRepository gamesRepository, IUsersRepository usersRepository)
        {
            _leaguesRepository = leaguesRepository;
            _teamsRepository = teamsRepository;
            _gamesRepository = gamesRepository;
            _usersRepository = usersRepository;
        }

        public IEnumerable<LeagueViewModel> GetAll()
        {
            return _leaguesRepository.GetAll().Select(x => new LeagueViewModel
            {
                Id = x.EntityId,
                Name = x.Name,
                Description = x.Description
            });
        }

        public LeagueViewModel Get(string leagueId)
        {
            var league = _leaguesRepository.Get(leagueId);
            return league == null
                ? null
                : new LeagueViewModel
                {
                    Id = league.EntityId,
                    Name = league.Name,
                    Description = league.Description
                };
        }

        public void Create(LeagueViewModel model)
        {
            _leaguesRepository.Create(model.Name, model.Description);
        }

        public void Update(LeagueViewModel model)
        {
            _leaguesRepository.Update(model.Id, model.Name, model.Description);
        }

        public LeagueStatisticViewModel GetStatisticByLeague(string leagueId)
        {
            var teams = _teamsRepository.GetByLeague(leagueId).ToList();
            var games = _gamesRepository.GetByLeague(leagueId).ToList();
            var teamsStatistic = new List<TeamStatisticViewModel>();
            var scoreCalculation = new ScoreCalculation();

            foreach (var team in teams)
            {
                var model = new TeamStatisticViewModel
                {
                    Id = team.EntityId,
                    Name = team.Name
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

                model.Points = scoreCalculation.Default(model.Wins, model.Loses, model.Draws);

                teamsStatistic.Add(model);
            }

            var bestPlayers = Enumerable.Concat(
                games.Where(x => !string.IsNullOrEmpty(x.HomeTeam.BestMemberId)).Select(x => x.HomeTeam.BestMemberId),
                games.Where(x => !string.IsNullOrEmpty(x.GuestTeam.BestMemberId)).Select(x => x.GuestTeam.BestMemberId))
                .GroupBy(x => x)
                .Select(x => new
                {
                    Id = x.Key,
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            var bestForwards = Enumerable.Concat(
                games.SelectMany(x => x.HomeTeam.Members),
                games.SelectMany(x => x.GuestTeam.Members))
                .GroupBy(x => x.Id)
                .Select(x => new
                {
                    Id = x.Key,
                    Goals = x.Sum(y => y.Score)
                })
                .OrderByDescending(x => x.Goals)
                .ToList();

            var bestHelpers = Enumerable.Concat(
                games.SelectMany(x => x.HomeTeam.Members),
                games.SelectMany(x => x.GuestTeam.Members))
                .GroupBy(x => x.Id)
                .Select(x => new
                {
                    Id = x.Key,
                    Helps = x.Sum(y => y.Help)
                })
                .OrderByDescending(x => x.Helps)
                .ToList();

            var userIds = new List<string>();
            userIds.AddRange(bestPlayers.Select(x => x.Id));
            userIds.AddRange(bestForwards.Select(x => x.Id));
            userIds.AddRange(bestHelpers.Select(x => x.Id));
            userIds = userIds.Distinct().ToList();
            var users = _usersRepository
                .GetRange(userIds)
                .ToDictionary(x => x.EntityId, 
                    y => teams.Exists(x => x.MemberIds.Contains(y.EntityId)) 
                        ? y.Name + " (" + teams.Find(x => x.MemberIds.Contains(y.EntityId)).Name + ")"
                        : y.Name);

            return new LeagueStatisticViewModel
            {
                BestPlayers = bestPlayers.Select(x => new IdNameViewModel
                {
                    Id = x.Id,
                    Name = users.ContainsKey(x.Id) 
                        ? users[x.Id] + " " + x.Count
                        : null
                }),
                BestForwards = bestForwards.Select(x => new IdNameViewModel
                {
                    Id = x.Id,
                    Name = users.ContainsKey(x.Id)
                        ? users[x.Id] + " " + x.Goals
                        : null
                }),
                BestHelpers = bestHelpers.Select(x => new IdNameViewModel
                {
                    Id = x.Id,
                    Name = users.ContainsKey(x.Id)
                        ? users[x.Id] + " " + x.Helps
                        : null
                }),
                TeamStatistics = teamsStatistic
                    .OrderByDescending(x => x.Points)
                    .ThenByDescending(x => x.Scores)
                    .ThenByDescending(x => x.Missed)
            };
        }

        public void Delete(string leagueId)
        {
            throw new System.NotImplementedException();
        }
    }
}
