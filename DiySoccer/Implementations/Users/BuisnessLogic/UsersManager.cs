using System.Collections.Generic;
using System.Linq;
using Implementations.Core.DataAccess;
using Interfaces.Teams.DataAccess;
using Interfaces.Users.BuisnessLogic;
using Interfaces.Users.DataAccess;
using MongoDB.Driver;

namespace Implementations.Users.BuisnessLogic
{
    public class UsersManager : IUsersManager
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITeamsRepository _teamsRepository;

        public UsersManager(IUsersRepository usersRepository, ITeamsRepository teamsRepository)
        {
            _usersRepository = usersRepository;
            _teamsRepository = teamsRepository;
        }

        public IEnumerable<UserDb> Find(string leagueId, string query, string excludeTeamId, int page, int pageSize)
        {
            var users = _usersRepository.Find(leagueId, query, 0, int.MaxValue).Select(x => x.EntityId).ToList();
            var teams = _teamsRepository
                .FindByUsers(leagueId, users)
                .Where(x => x.EntityId != excludeTeamId)
                .ToList();

            var usersNotFromtheTeam = users
                .Where(x =>
                    teams.Exists(team => team.EntityId != excludeTeamId && team.MemberIds.Contains(x)));

        }
    }
}
