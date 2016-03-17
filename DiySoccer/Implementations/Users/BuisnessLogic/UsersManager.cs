using System.Collections.Generic;
using System.Linq;
using Implementations.Core.DataAccess;
using Interfaces.Core;
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

        public IEnumerable<IdNameViewModel> Find(string leagueId, string query, IEnumerable<string> excludeTeamsId, int page, int pageSize)
        {
            var users = _usersRepository.Find(leagueId, query, 0, int.MaxValue).ToList();
            var userIds = users.Select(x => x.EntityId).ToList();

            var teams = _teamsRepository
                .FindByUsers(leagueId, userIds)
                .Where(x => !excludeTeamsId.Contains(x.EntityId))
                .ToList();
            
            var result = new List<IdNameViewModel>();
            foreach (var user in users)
            {
                var userTeam = teams.FirstOrDefault(team => team.MemberIds.Contains(user.EntityId));
                var model = new IdNameViewModel
                {
                    Id = user.EntityId,
                    Name = userTeam == null
                        ? user.Name
                        : user.Name + " (" + userTeam.Name + ")"
                };

                result.Add(model);
            }
            return result;
        }
    }
}
