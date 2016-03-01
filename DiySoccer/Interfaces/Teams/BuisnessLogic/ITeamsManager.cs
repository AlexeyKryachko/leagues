using Interfaces.Teams.BuisnessLogic.Models;

namespace Interfaces.Teams.BuisnessLogic
{
    public interface ITeamsManager
    {
        void Create(CreateTeamViewModel model);
    }
}
