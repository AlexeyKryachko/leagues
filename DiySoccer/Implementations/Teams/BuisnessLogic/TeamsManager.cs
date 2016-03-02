using System.Collections.Generic;
using System.Linq;
using Interfaces.Core;
using Interfaces.Teams.BuisnessLogic;
using Interfaces.Teams.BuisnessLogic.Models;
using Interfaces.Teams.DataAccess;
using Interfaces.Users.DataAccess;

namespace Implementations.Teams.BuisnessLogic
{
    public class TeamsManager : ITeamsManager
    {
        private readonly ITeamsRepository _teamsRepository;
        private readonly IUsersRepository _usersRepository;
        
        public TeamsManager(ITeamsRepository teamsRepository, IUsersRepository usersRepository)
        {
            _teamsRepository = teamsRepository;
            _usersRepository = usersRepository;
        }

        public void Create(CreateTeamViewModel model)
        {
            var userEntities = model.Members.Select(x => new UserDb {Name = x.Value}).ToList();
            _usersRepository.AddRange(userEntities);

            var userIds = userEntities.Select(x => x.Id);
            _teamsRepository.Create(model.League, model.Name, userIds);
        }

        public IEnumerable<TeamViewModel> GetByLeague(string id)
        {
            var teams = _teamsRepository.GetByLeague(id).ToList();
            var userIds = teams.SelectMany(x => x.MemberIds).Distinct();
            var users = _usersRepository.GetRange(userIds).ToList()
                .ToDictionary(x => x.Id, y => y);

            return teams.Select(x => new TeamViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Members = x.MemberIds.Select(y => new IdValueViewModel
                {
                    Id = y,
                    Value = users[y].Name
                })
            });
        }
    }
}
