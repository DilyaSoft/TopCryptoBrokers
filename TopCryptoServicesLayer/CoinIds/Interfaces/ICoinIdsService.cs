using System.Threading.Tasks;

namespace TopCrypto.ServicesLayer.CoinIds.Interfaces
{
    public interface ICoinIdsService
    {
        Task<bool> IsCacheFresh();
        Task UpdateDBCacheForAll();
    }
}