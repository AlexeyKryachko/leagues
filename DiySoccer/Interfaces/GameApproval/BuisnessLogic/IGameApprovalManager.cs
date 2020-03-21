using Interfaces.Games.BuisnessLogic.Models;

namespace Interfaces.GameApproval.BuisnessLogic
{
    public interface IGameApprovalManager
    {
        void Create(string lueagueId, GameExternalViewModel model);
    }
}
