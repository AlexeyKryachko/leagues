using Newtonsoft.Json;

namespace Interfaces.Core.Services.Medias.BuisnessLogic
{
    public class MediaViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
