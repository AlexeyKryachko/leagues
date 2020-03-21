using Implementations.Core.DataAccess;
using Interfaces.GameApproval.DataAccess;
using Interfaces.GameApproval.DataAccess.Model;
using Interfaces.Games.DataAccess.Model;

namespace Implementations.GameApproval.DataAccess
{
    public class GameApprovalRepository : BaseRepository<GameApprovalDb>, IGameApprovalRepository
    {
        protected override string CollectionName => "approvals";
    }
}
