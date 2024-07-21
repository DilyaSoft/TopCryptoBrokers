using Newtonsoft.Json;

namespace TopCrypto.ServicesLayer.Settings.Models
{
    public class SettingAdminDTO
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("query")]
        public string Query { get; set; }
    }
}
