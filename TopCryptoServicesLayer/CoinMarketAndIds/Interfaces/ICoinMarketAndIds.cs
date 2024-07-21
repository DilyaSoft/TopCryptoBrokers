using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.ServicesLayer.CoinMarketAndIds.Models;

namespace TopCrypto.ServicesLayer.CoinMarketAndIds.Interfaces
{
    public interface ICoinMarketAndIds
    {
        Task CleanDictinaryCache();
        Task<IEnumerable<CoinIdsListDTO>> GetListOfIdsWithMarkets();
    }
}