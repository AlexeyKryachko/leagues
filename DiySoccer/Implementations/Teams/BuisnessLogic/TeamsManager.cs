using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;
using Interfaces.Games.DataAccess;
using Interfaces.Teams.BuisnessLogic;
using Interfaces.Teams.BuisnessLogic.Models;
using Interfaces.Teams.DataAccess;
using Interfaces.Users.DataAccess;

namespace Implementations.Teams.BuisnessLogic
{
    public class TeamsManager : ITeamsManager
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly ITeamsRepository _teamsRepository;
        private readonly IUsersRepository _usersRepository;
        
        public TeamsManager(ITeamsRepository teamsRepository, IUsersRepository usersRepository, IGamesRepository gamesRepository)
        {
            _teamsRepository = teamsRepository;
            _usersRepository = usersRepository;
            _gamesRepository = gamesRepository;
        }

        public void Create(string leagueId, CreateTeamViewModel model)
        {
            var exitedIds = model.Members
                .Where(x => !string.IsNullOrEmpty(x.Id))
                .Select(x => x.Id);
            var userEntities = model.Members
                .Where(x => string.IsNullOrEmpty(x.Id))
                .Select(x => new UserDb {Name = x.Name})
                .ToList();
            _usersRepository.AddRange(leagueId, userEntities);

            var userIds = userEntities.Select(x => x.EntityId);
            var memberIds = exitedIds.Concat(userIds);
            _teamsRepository.Create(leagueId, model.Name, memberIds);
        }

        public TeamViewModel Get(string leagueId, string teamId)
        {
            var team = _teamsRepository.Get(teamId);
            if (team == null)
                return null;

            var members = _usersRepository.GetRange(team.MemberIds);
            return new TeamViewModel
            {
                Id = team.EntityId,
                Name = team.Name,
                Members = members.Select(x => new IdNameViewModel
                {
                    Id = x.EntityId,
                    Name = x.Name
                })
            };
        }

        public TeamInfoViewModel GetInfo(string leagueId, string teamId)
        {
            var games = _gamesRepository.GetByTeam(leagueId, teamId);
            var teamIds = games.Select(x => x.GuestTeam.Id).Concat(games.Select(x => x.HomeTeam.Id)).ToList();
            teamIds.Add(teamId);
            teamIds = teamIds.Distinct().ToList();
            var teams = _teamsRepository
                .GetRange(teamIds)
                .ToDictionary(x => x.EntityId, y => y.Name);

            return new TeamInfoViewModel
            {
                Id = teamId,
                Name = teams[teamId],
                Games = games.Select(x => new TeamInfoGameViewModel
                {
                    Id = x.EntityId,
                    OpponentName = x.GuestTeam.Id == teamId
                        ? teams[x.HomeTeam.Id]
                        : teams[x.GuestTeam.Id],
                    Goals = x.GuestTeam.Id == teamId
                        ? x.HomeTeam.Score + ":" + x.GuestTeam.Score + "*"
                        : x.GuestTeam.Score + ":" + x.HomeTeam.Score + "*"
                })
            };
        }

        public void Update(string leagueId, string teamId, CreateTeamViewModel model)
        {
            var exitedIds = model.Members
                .Where(x => !string.IsNullOrEmpty(x.Id))
                .Select(x => x.Id);
            var userEntities = model.Members
                .Where(x => string.IsNullOrEmpty(x.Id))
                .Select(x => new UserDb { Name = x.Name })
                .ToList();
            _usersRepository.AddRange(leagueId, userEntities);

            var userIds = userEntities.Select(x => x.EntityId);
            var memberIds = exitedIds.Concat(userIds);
            _teamsRepository.Update(leagueId, teamId, model.Name, memberIds);
        }

        public IEnumerable<TeamViewModel> GetByLeague(string id)
        {
            var teams = _teamsRepository.GetByLeague(id).ToList();
            var userIds = teams.SelectMany(x => x.MemberIds).Distinct();
            var users = _usersRepository.GetRange(userIds).ToDictionary(x => x.EntityId, y => y);

            return teams.Select(x => new TeamViewModel
            {
                Id = x.EntityId,
                Name = x.Name,
                Members = x.MemberIds.Select(y => new IdNameViewModel
                {
                    Id = y,
                    Name = users[y].Name
                })
            });
        }

        public IEnumerable<TeamStatisticViewModel> GetStatisticByLeague(string leagueId)
        {
            var teams = _teamsRepository.GetByLeague(leagueId).ToList();
            var games = _gamesRepository.GetByLeague(leagueId).ToList();
            var result = new List<TeamStatisticViewModel>();
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

                result.Add(model);
            }
            
            return result
                .OrderByDescending(x => x.Points)
                .ThenByDescending(x => x.Scores)
                .ThenByDescending(x => x.Missed);
        }
    }
}
