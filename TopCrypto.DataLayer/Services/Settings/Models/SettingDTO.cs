using Newtonsoft.Json;

namespace TopCrypto.DataLayer.Services.Settings.Models
{
    public class SettingDTO
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("query")]
        public string Query { get; set; }

        //testings 
        public override string ToString()
        {
            return string.Format("{{SettingDTO Id:{0}  Value:{1}  Query:{2} }}", Id, Value, Query);
        }
    }
}
