using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinInfo.Models;

namespace TopCrypto.DataLayer.Services.CoinInfo.Interfaces
{
    public interface ICoinInfoPricesDataService
    {
        Task SaveNewResult(IList<CoinInfoDTO> coinInfoList);
        Task<IList<CryptoCurencyPriceDTO>> GetAverageWeekValues();
    }
}