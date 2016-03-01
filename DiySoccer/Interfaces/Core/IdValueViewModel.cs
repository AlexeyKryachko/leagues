using Newtonsoft.Json;

namespace Interfaces.Core
{
    public class IdValueViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
