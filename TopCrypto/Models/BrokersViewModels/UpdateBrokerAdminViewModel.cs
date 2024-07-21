using Newtonsoft.Json;
using TopCrypto.DataLayer.Services.Broker.Models;

namespace TopCrypto.Models.BrokersViewModels
{
    public class UpdateBrokerAdminViewModel : BrokerAdminDTO
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}
