using Newtonsoft.Json;

namespace TopCrypto.DataLayer.Services.News.Models
{
    public class NewsLocalizationDTO
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("isHidden")]
        public bool IsHidden { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
        [JsonProperty("previewBody")]
        public string PreviewBody { get; set; }
    }
}
