using Newtonsoft.Json;

namespace TopCrypto.DataLayer.Services.FiatCurrency.Models
{
    public class FiatCurrencyDTO
    {
        public FiatCurrencyDTO() { }
        public FiatCurrencyDTO(string name, double val)
        {
            this.Name = name;
            this.Value = val;
        }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("value")]
        public double Value { get; set; }
    }
}
