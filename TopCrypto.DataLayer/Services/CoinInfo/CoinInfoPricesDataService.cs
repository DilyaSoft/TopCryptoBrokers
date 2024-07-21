using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using TopCrypto.DataLayer.Services.CoinInfo.Interfaces;
using TopCrypto.DataLayer.Services.CoinInfo.Models;
using TopCrypto.DataLayer.Services.Interfaces;

namespace TopCrypto.DataLayer.Services.CoinInfo
{
    public class CoinInfoPricesDataService : AbstractSQLDataService, ICoinInfoPricesDataService
    {
        public CoinInfoPricesDataService(IConfiguration config) : base(config)
        {
        }

        public async Task SaveNewResult(IList<CoinInfoDTO> coinInfoList)
        {
            var jarray = new JArray();
            foreach (var coin in coinInfoList)
            {
                jarray.Add(new JObject
                {
                    ["cryptoId"] = coin.Id,
                    ["price"] = coin.PriceUsd,
                    ["lastUpdatedApi"] = coin.LastUpdated
                });
            }

            await SaveNewResultAndGetOld(jarray.ToString());
        }

        private async Task SaveNewResultAndGetOld(string jsonValue)
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand("Add_records_into_CryptoCurrencyPrices", conn))
                {
                    comm.CommandTimeout = 60 * 3;
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@JsonValue", jsonValue);

                    await conn.OpenAsync();
                    await comm.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<IList<CryptoCurencyPriceDTO>> GetAverageWeekValues()
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand("GetWeekAverageValues", conn)
                {
                    CommandTimeout = 60 * 5,
                    CommandType = CommandType.StoredProcedure
                })
                {
                    await conn.OpenAsync();
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        var list = new List<CryptoCurencyPriceDTO>();
                        if (!reader.HasRows) return list;
                        while (await reader.ReadAsync())
                        {
                            list.Add(new CryptoCurencyPriceDTO()
                            {
                                Price = (double)reader["price"],
                                CryptoId = (int)reader["cryptoId"]
                            });
                        }

                        return list;
                    }
                }
            }
        }
    }
}
