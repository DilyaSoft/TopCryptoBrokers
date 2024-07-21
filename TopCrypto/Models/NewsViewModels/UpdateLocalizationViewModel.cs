using Newtonsoft.Json;
using TopCrypto.DataLayer.Services.News.Models;

namespace TopCrypto.Models.NewsViewModels
{
    public class UpdateLocalizationViewModel : NewsLocalizationDTO
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "culture")]
        public string Culture { get; set; }
    }
}
