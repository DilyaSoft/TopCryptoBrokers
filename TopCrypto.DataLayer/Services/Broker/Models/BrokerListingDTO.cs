using System.Collections.Generic;

namespace TopCrypto.DataLayer.Services.Broker.Models
{
    public class BrokerListingDTO
    {
        public BrokerListingDTO()
        {
            BrokerTopDTOList = new List<BrokerTopDTO>();
        }

        public IList<BrokerTopDTO> BrokerTopDTOList { get; set; }
        public int CountVisibleTopBrokers { get; set; }
    }
}
