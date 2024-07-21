using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinInfo.Models;

namespace TopCrypto.ServicesLayer.CoinInfo.Interfaces
{
    public interface ICryptoCurencyPriceSaver
    {
        Task SaveNewResult(IList<CoinInfoDTO> coinInfoList);
        Dictionary<int, List<double>> GetWeekPriceAverage(out bool isFromCache
            , bool ignoreCacheExpiration = true);
        Dictionary<int, List<double>> GetWeekPriceAverage(IEnumerable<string> ids);
        Dictionary<int, List<double>> GetWeekPriceAverage(IEnumerable<string> ids
            , out bool isFromCache
            , bool ignoreCacheExpiration);
        Dictionary<int, List<double>> GetWeekPriceAverage(IEnumerable<string> ids
            , Dictionary<int, List<double>> priceAverage);
    }
}
