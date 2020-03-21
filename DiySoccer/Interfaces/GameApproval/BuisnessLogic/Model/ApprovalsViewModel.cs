using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interfaces.GameApproval.BuisnessLogic.Model
{
    public class ApprovalsViewModel
    {
        [JsonProperty("approvals")]
        public List<ApprovalViewModel> Approvals;

        public ApprovalsViewModel()
        {
            Approvals = new List<ApprovalViewModel>();
        }
    }
}
