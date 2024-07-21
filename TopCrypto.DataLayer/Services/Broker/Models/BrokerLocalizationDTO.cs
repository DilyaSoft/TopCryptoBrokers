using Newtonsoft.Json;

namespace TopCrypto.DataLayer.Services.Broker.Models
{
    public class BrokerLocalizationDTO
    {
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "instruments")]
        public string Instruments { get; set; }

        [JsonProperty(PropertyName = "regulatedby")]
        public string RegulatedBy { get; set; }

        [JsonProperty(PropertyName = "urlimg")]
        public string UrlImg { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "visible")]
        public bool Visible { get; set; }

        [JsonProperty(PropertyName = "mindepositlisting")]
        public string MinDepositListing { get; set; }

        [JsonProperty(PropertyName = "regulationlisting")]
        public string RegulationListing { get; set; }

        [JsonProperty(PropertyName = "spreadslisting")]
        public string SpreadsListing { get; set; }

        [JsonProperty(PropertyName = "regulation")]
        public string Regulation { get; set; }

        [JsonProperty(PropertyName = "businessnature")]
        public string BusinessNature { get; set; }

        [JsonProperty(PropertyName = "leverage")]
        public string Leverage { get; set; }

        [JsonProperty(PropertyName = "platforms")]
        public string Platforms { get; set; }

        [JsonProperty(PropertyName = "accounttypes")]
        public string AccountTypes { get; set; }

        [JsonProperty(PropertyName = "lotsizes")]
        public string LotSizes { get; set; }

        [JsonProperty(PropertyName = "demoaccount")]
        public string DemoAccount { get; set; }

        [JsonProperty(PropertyName = "accountcurrency")]
        public string AccountCurrency { get; set; }

        [JsonProperty(PropertyName = "pairsoffered")]
        public string PairsOffered { get; set; }

        [JsonProperty(PropertyName = "paymentmethods")]
        public string PaymentMethods { get; set; }

        [JsonProperty(PropertyName = "commissions")]
        public string Commissions { get; set; }

        [JsonProperty(PropertyName = "additionaltext")]
        public string AdditionalText { get; set; }

        [JsonProperty(PropertyName = "shortinfo1")]
        public string ShortInfo1 { get; set; }

        [JsonProperty(PropertyName = "shortinfo2")]
        public string ShortInfo2 { get; set; }

        [JsonProperty(PropertyName = "endinfo")]
        public string EndInfo { get; set; }
    }
}
