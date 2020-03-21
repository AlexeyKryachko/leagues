using Newtonsoft.Json;

namespace Interfaces.GameApproval.BuisnessLogic.Model
{
    public class PlayerApprovalViewModel
    {
        [JsonProperty("name")]
        public string Name;
        
        [JsonProperty("score")]
        public int Score;

        [JsonProperty("help")]
        public int Help;

        [JsonProperty("best")]
        public bool Best;
    }
}
