using Newtonsoft.Json;
using TopCrypto.DataLayer.Services.Broker.Models;

namespace TopCrypto.Models.BrokersViewModels
{
    public class UpdateBrokerLocalizationViewModel : BrokerLocalizationDTO
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "culture")]
        public string Culture { get; set; }
    }
}
