using Newtonsoft.Json;
namespace TopCrypto.DataLayer.Services.Broker.Models
{
    public class BrokerAdminDTO
    {
        [JsonProperty(PropertyName = "originalname")]
        public string OriginalName { get; set; }
        [JsonProperty(PropertyName = "mark")]
        public int? Mark { get; set; }
        [JsonProperty(PropertyName = "foundation")]
        public int? Foundation { get; set; }
        [JsonProperty(PropertyName = "phonenumbershtml")]
        public string PhoneNumbersHtml { get; set; }
        [JsonProperty(PropertyName = "emailhtml")]
        public string EmailHtml { get; set; }
    }
}
