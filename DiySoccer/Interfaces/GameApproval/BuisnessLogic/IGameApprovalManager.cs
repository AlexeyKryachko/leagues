using Interfaces.GameApproval.BuisnessLogic.Model;
using Interfaces.Games.BuisnessLogic.Models;

namespace Interfaces.GameApproval.BuisnessLogic
{
    public interface IGameApprovalManager
    {
        ApprovalsViewModel GetApprovals(string leagueId);
        void Create(string lueagueId, GameExternalViewModel model);
        void Approve(string lueagueId, string id);
        void Delete(string id);
    }
}
