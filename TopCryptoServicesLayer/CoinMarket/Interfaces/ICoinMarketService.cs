using System.Threading.Tasks;

namespace TopCrypto.ServicesLayer.CoinMarket.Interfaces
{
    public interface ICoinMarketService
    {
        Task<bool> IsCacheFresh();
        Task UpdateDBCache();
    }
}