using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace TopCrypto.DataLayer.Services.Broker.Models
{
    public class BrokerDTO
    {
        [JsonProperty(PropertyName = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? Mark { get; set; }
        public int? Foundation { get; set; }

        public string UrlImg { get; set; }
        public string Name { get; set; }
        public string RegulatedBy { get; set; }
        public string BusinessNature { get; set; }
        public string Leverage { get; set; }
        public string Instruments { get; set; }
        public string Platforms { get; set; }
        public string AccountTypes { get; set; }
        public string LotSizes { get; set; }
        public string DemoAccount { get; set; }
        public string AccountCurrency { get; set; }
        public string PairsOffered { get; set; }
        public string PaymentMethods { get; set; }
        public string Commissions { get; set; }

        public string AdditionalText { get; set; }
        public string ShortInfo1 { get; set; }
        public string ShortInfo2 { get; set; }
        public string EndInfo { get; set; }

        public string PhoneNumbersHtml { get; set; }
        public string EmailHtml { get; set; }
    }
}
