using Newtonsoft.Json;

namespace TopCrypto.ServicesLayer.Settings.Models
{
    public class SettingFromConfigDTO
    {
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("{{Label:{0}   Name:{1} }}", Label, Name);
        }
    }
}
