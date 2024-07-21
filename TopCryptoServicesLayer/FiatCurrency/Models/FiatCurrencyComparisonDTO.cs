using TopCrypto.DataLayer.Services.FiatCurrency.Models;
using TopCrypto.ServicesLayer.SocketServices.WebSockets;
using TopCrypto.ServicesLayer.WebSockets.Models;

namespace TopCrypto.ServicesLayer.FiatCurrency.Models
{
    public class FiatCurrencyComparisonDTO : FiatCurrencyDTO, IComparisonDTO
    {
        public SocketComparisonEnum Code { get; set; }
    }
}
