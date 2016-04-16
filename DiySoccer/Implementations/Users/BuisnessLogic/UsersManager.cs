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
        private readonly IPlayersRepository _playersRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly ITeamsRepository _teamsRepository;

        public UsersManager(IPlayersRepository playersRepository, ITeamsRepository teamsRepository, IUsersRepository usersRepository)
        {
            _playersRepository = playersRepository;
            _teamsRepository = teamsRepository;
            _usersRepository = usersRepository;
        }

        public IEnumerable<IdNameViewModel> FindPlayer(string leagueId, string query, IEnumerable<string> excludeTeamsId, int page, int pageSize)
        {
            var users = _playersRepository.Find(leagueId, query, 0, int.MaxValue).ToList();
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

        public IEnumerable<IdNameViewModel> FindUser(string query, int page, int pageSize)
        {
            return _usersRepository.Find(query, 0, int.MaxValue).Select(x => new IdNameViewModel
            {
                Id = x.Id,
                Name = x.UserName
            });
        }
    }
}
