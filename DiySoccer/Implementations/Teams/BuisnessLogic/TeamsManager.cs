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

        public void Create(CreateTeamViewModel model)
        {
            var userEntities = model.Members.Select(x => new UserDb {Name = x.Value}).ToList();
            _usersRepository.AddRange(userEntities);

            var userIds = userEntities.Select(x => x.EntityId);
            _teamsRepository.Create(model.League, model.Name, userIds);
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
                Members = x.MemberIds.Select(y => new IdValueViewModel
                {
                    Id = y,
                    Value = users[y].Name
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

            return result;
        }
    }
}
