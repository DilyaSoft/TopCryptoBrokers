using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinMarket.Models;

namespace TopCrypto.DataLayer.Services.CoinMarket.Interfaces
{
    public interface ICoinMarketDataService
    {
        Task ClearAndInsertNewCache(IList<CoinMarketDTO> marketDTOs);
        Task<IList<CoinMarketDTO>> GetDataFromDBCache();
    }
}