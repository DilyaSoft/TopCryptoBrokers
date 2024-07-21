using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinGraph.Models;

namespace TopCrypto.ServicesLayer.CoinGraph.Interfaces
{
    public interface ICoinInfoGraphService
    {
        Task<List<CoinInfoGraphDTO>> GetCoinApiDTOFromCache(string coinId, string timeType, DateTime? dt1, DateTime? dt2);
        Task<bool> IsCacheFresh();
        Task UpdateDBCacheForAll();
    }
}