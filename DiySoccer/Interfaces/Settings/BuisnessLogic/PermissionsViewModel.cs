using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interfaces.Settings.BuisnessLogic
{
    public class PermissionsViewModel
    {
        [JsonProperty("isAuthenticated")]
        public bool IsAuthenticated { get; set; }

        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }

        [JsonProperty("relationships")]
        public Dictionary<string, string> Relationships { get; set; }

        public PermissionsViewModel()
        {
            Relationships = new Dictionary<string, string>();
        }
    }
}
