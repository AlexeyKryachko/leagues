using Newtonsoft.Json;

namespace Interfaces.Core
{
    public class IdNameViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
