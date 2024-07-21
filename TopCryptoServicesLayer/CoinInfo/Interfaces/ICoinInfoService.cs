using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinGraph.Models;
using TopCrypto.DataLayer.Services.CoinIds.Models;
using TopCrypto.DataLayer.Services.CoinInfo.Models;
using TopCrypto.ServicesLayer.Interfaces;

namespace TopCrypto.ServicesLayer.CoinInfo.Interfaces
{
    public interface ICoinInfoService : IFilterService<CoinInfoDTO>
    {
        Task<IList<CoinIdsDTO>> GetAllCoinsID();
        Task<IEnumerable<CryptoCurrencyDTO>> GetDataBaseIds();
        Task ClearAndInsertNewCryptoCurrency(CryptoCurrencyDTO[] arr);
    }
}