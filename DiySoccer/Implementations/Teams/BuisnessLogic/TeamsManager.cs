using System.Linq;
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
    }
}
