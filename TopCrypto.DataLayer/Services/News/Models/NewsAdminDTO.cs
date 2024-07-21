using Newtonsoft.Json;
using System;

namespace TopCrypto.DataLayer.Services.News.Models
{
    public class NewsAdminDTO
    {
        [JsonProperty(PropertyName = "dateForShowing")]
        public DateTime? DateForShowing { get; set; }

        [JsonProperty(PropertyName = "imgLink")]
        public string ImgLink { get; set; }

        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }
    }
}
