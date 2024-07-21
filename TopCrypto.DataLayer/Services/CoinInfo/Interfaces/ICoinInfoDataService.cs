using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinGraph.Models;
using TopCrypto.DataLayer.Services.CoinInfo.Models;
using TopCrypto.DataLayer.Services.Interfaces;

namespace TopCrypto.DataLayer.Services.CoinInfo.Interfaces
{
    public interface ICoinInfoDataService : IСurrencyDataService
    {
        Task ClearAndInsertNewCryptoCurrency(CryptoCurrencyDTO[] listOfCurrency);
        Task<IList<CoinIdNameDTO>> GetAllCoinsIDFromDBExistedInDB();
    }
}