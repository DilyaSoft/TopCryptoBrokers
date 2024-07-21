using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinGraph.Models;

namespace TopCrypto.DataLayer.Services.CoinGraph.Interfaces
{
    public interface ICoinInfoGraphDataService
    {
        Task<List<CoinInfoGraphDTO>> GetCoinInfoGraphDTOs(string coinId
            , TimePeriod period
            , DateTime? dt1
            , DateTime? dt2);
        Task<List<CryptoCurrencyDTO>> GetCryptoСurrency();
        Task<CryptoCurrencyDTO> GetCryptoСurrencyById(string coinId);
        Task UpdateCacheData(List<CoinInfoGraphDTO> dtoList
            , string coinId
            , TimePeriod timeType
            , string marketId);
        Task<DateTime> GetLastStartDateAndDeleteRow(string coinId, TimePeriod period, string coin_graph_id);
    }
}