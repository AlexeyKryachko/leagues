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

        public void Create(string leagueId, TeamViewModel model)
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

            var entity = new TeamDb
            {
                LeagueId = leagueId,
                Name = model.Name,
                Hidden = model.Hidden,
                MemberIds = memberIds,
                MediaId = model.Media
            };
            _teamsRepository.Add(leagueId, entity);
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
                Hidden = team.Hidden,
                Members = members.Select(x => new IdNameViewModel
                {
                    Id = x.EntityId,
                    Name = x.Name
                }),
                Media = team.MediaId
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

        public void Update(string leagueId, string teamId, TeamViewModel model)
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

            var entity = new TeamDb
            {
                EntityId = teamId,
                LeagueId = leagueId,
                Name = model.Name,
                Hidden = model.Hidden,
                MemberIds = memberIds,
                MediaId = model.Media
            };
            _teamsRepository.Update(leagueId, entity);
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
                Hidden = x.Hidden,
                Members = x.MemberIds.Select(y => new IdNameViewModel
                {
                    Id = y,
                    Name = users[y].Name
                })
            });
        }
    }
}
