using Newtonsoft.Json;

namespace Interfaces.Shared
{
    public class BreadcrumpViewModel
    {
        [JsonProperty("type")]
        public BreadcrumpType Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
