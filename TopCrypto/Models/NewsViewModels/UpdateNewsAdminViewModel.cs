using Newtonsoft.Json;
using TopCrypto.DataLayer.Services.News.Models;

namespace TopCrypto.Models.NewsViewModels
{
    public class UpdateNewsAdminViewModel : NewsAdminDTO
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}
