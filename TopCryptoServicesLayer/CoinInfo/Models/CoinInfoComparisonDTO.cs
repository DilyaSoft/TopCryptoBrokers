using TopCrypto.DataLayer.Services.CoinInfo.Models;
using TopCrypto.ServicesLayer.SocketServices.WebSockets;
using TopCrypto.ServicesLayer.WebSockets.Models;

namespace TopCrypto.ServicesLayer.CoinInfo.Models
{
    public class CoinInfoComparisonDTO : CoinInfoDTO, IComparisonDTO
    {
        public SocketComparisonEnum Code { get; set; }
    }
}
