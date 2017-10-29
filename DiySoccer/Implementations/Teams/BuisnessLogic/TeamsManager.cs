using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;
using Interfaces.Events.DataAccess;
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
        private readonly IPlayersRepository _playersRepository;
        private readonly IEventsRepository _eventsRepository;

        private readonly TeamsMapper _teamMapper;

        public TeamsManager(ITeamsRepository teamsRepository, 
            IPlayersRepository playersRepository, 
            IGamesRepository gamesRepository, 
            TeamsMapper teamMapper, 
            IEventsRepository eventsRepository)
        {
            _teamsRepository = teamsRepository;
            _playersRepository = playersRepository;
            _gamesRepository = gamesRepository;
            _teamMapper = teamMapper;
            _eventsRepository = eventsRepository;
        }

        public IEnumerable<IdNameViewModel> Find(string query, int page, int pageSize)
        {
            return _teamsRepository
                .Find(query, page, pageSize)
                .Select(_teamMapper.MapIdName);
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
            _playersRepository.AddRange(leagueId, userEntities);

            var userIds = userEntities.Select(x => x.EntityId);
            var memberIds = exitedIds.Concat(userIds);

            var entity = new TeamDb
            {
                LeagueId = leagueId,
                Name = model.Name,
                Hidden = model.Hidden,
                MemberIds = memberIds,
                MediaId = model.Media,
                Description = model.Description
            };
            _teamsRepository.Add(leagueId, entity);
        }

        public TeamViewModel Copy(string teamId, string destinationUnionId)
        {
            var team = _teamsRepository.Get(teamId);
            if (team == null)
                return null;

            var newTeam = _teamsRepository.Create(destinationUnionId);
            team.ReferenceId = team.EntityId;
            team.EntityId = newTeam.EntityId;

            _teamsRepository.Update(destinationUnionId, team);

            var members = _playersRepository
                .GetRange(team.MemberIds)
                .ToDictionary(x => x.EntityId, x => x.Name);

            return _teamMapper.Map(team, members);
        }

        public TeamViewModel Get(string leagueId, string teamId)
        {
            var team = _teamsRepository.Get(teamId);
            if (team == null)
                return null;

            var members = _playersRepository
                .GetRange(team.MemberIds)
                .ToDictionary(x => x.EntityId, x => x.Name);

            return _teamMapper.Map(team, members);
        }

        public TeamInfoViewModel GetInfo(string leagueId, string teamId)
        {
            var events = _eventsRepository.GetByLeague(leagueId).ToList();
            var games = _gamesRepository.GetByTeam(leagueId, teamId).ToList();
            var teams = _teamsRepository
                .GetByLeague(leagueId)
                .ToDictionary(x => x.EntityId, y => y);

            var userIds = teams[teamId].MemberIds;
            var users = _playersRepository.GetRange(userIds).ToList();

            return _teamMapper.MapTeamInfoViewModel(games, teamId, teams, users, events);
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
            _playersRepository.AddRange(leagueId, userEntities);

            var userIds = userEntities.Select(x => x.EntityId);
            var memberIds = exitedIds.Concat(userIds);

            var entity = new TeamDb
            {
                EntityId = teamId,
                LeagueId = leagueId,
                Name = model.Name,
                Hidden = model.Hidden,
                MemberIds = memberIds,
                MediaId = model.Media,
                Description = model.Description
            };

            _teamsRepository.Update(leagueId, entity);
        }
        
        public IEnumerable<TeamViewModel> GetByLeague(string id)
        {
            var teams = _teamsRepository.GetByLeague(id).ToList();
            var userIds = teams.SelectMany(x => x.MemberIds).Distinct();
            var users = _playersRepository.GetRange(userIds).ToDictionary(x => x.EntityId, y => y);

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
