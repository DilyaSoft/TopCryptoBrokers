using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using TopCrypto.DataLayer.Services.CoinGraph.Interfaces;
using TopCrypto.DataLayer.Services.CoinGraph.Models;
using TopCrypto.DataLayer.Services.Interfaces;

namespace TopCrypto.DataLayer.Services.CoinGraph
{
    public enum TimePeriod
    {
        DAY = 1,
        MONTH = 2,
        YEAR = 3
    }

    public class CoinInfoGraphDataService : AbstractSQLDataService, ICoinInfoGraphDataService
    {
        public CoinInfoGraphDataService(IConfiguration config)
            : base(config){}

        public async Task UpdateCacheData(List<CoinInfoGraphDTO> dtoList
            , string coinId
            , TimePeriod timeType
            , string marketId)
        {
            if (dtoList == null || dtoList.Count == 0 || String.IsNullOrWhiteSpace(coinId)) return;

            using (var tx = new TransactionScope(TransactionScopeOption.RequiresNew
                , new TimeSpan(0, 15, 0)
                , TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var conn = new SqlConnection(GetDefaultConnectionString()))
                {
                    await conn.OpenAsync();
                    foreach (var dto in dtoList)
                    {
                        await InsertNewCache(coinId, timeType, conn, dto, marketId);

                    }
                }

                tx.Complete();
            }
        }

        private async Task InsertNewCache(string coinId
            , TimePeriod timeType
            , SqlConnection conn
            , CoinInfoGraphDTO dto
            , string marketId)
        {
            if (string.IsNullOrWhiteSpace(coinId) || dto == null) return;

            using (var comm = new SqlCommand(@"INSERT INTO [dbo].[CoinApiCache]
           ([date]
           ,[price]
           ,[coin_Id]
           ,[time_type]
           , coin_graph_id)
     VALUES
           (@date
           ,@price
           ,@coin_Id
           ,@time_type
           ,@coin_graph_id)", conn))
            {
                comm.CommandType = CommandType.Text;

                var param = comm.Parameters;
                param.AddWithValue("@date", dto.CloseTime);
                param.AddWithValue("@price", dto.ClosePrice);
                param.AddWithValue("@coin_Id", coinId);
                param.AddWithValue("@time_type", timeType);
                param.AddWithValue("@coin_graph_id", marketId);

                await comm.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<CryptoCurrencyDTO>> GetCryptoСurrency()
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"SELECT [Id]
      ,[coin_abb]
      ,[coin_graph_id]
  FROM [dbo].[CryptoСurrency]
    order by ID", conn))
                {
                    comm.CommandType = CommandType.Text;

                    await conn.OpenAsync();
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        var list = new List<CryptoCurrencyDTO>();
                        if (!reader.HasRows) return list;
                        while (await reader.ReadAsync())
                        {
                            list.Add(new CryptoCurrencyDTO()
                            {
                                Id = GetInt(reader, "id"),
                                Abb = GetNullOrString(reader, "coin_abb"),
                                MarketId = GetNullOrString(reader, "coin_graph_id")
                            });
                        }

                        return list;
                    }
                }
            }
        }

        public async Task<CryptoCurrencyDTO> GetCryptoСurrencyById(string coinId)
        {
            if (string.IsNullOrWhiteSpace(coinId)) return null;

            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"SELECT [Id]
      ,[coin_abb]
      ,[coin_graph_id]
  FROM [dbo].[CryptoСurrency]
    where Id = @Id", conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@Id", coinId);

                    await conn.OpenAsync();
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows) return null;
                        await reader.ReadAsync();

                        return new CryptoCurrencyDTO()
                        {
                            Id = GetInt(reader, "id"),
                            Abb = GetNullOrString(reader, "coin_abb"),
                            MarketId = GetNullOrString(reader, "coin_graph_id")
                        };
                    }
                }
            }
        }

        public async Task<List<CoinInfoGraphDTO>> GetCoinInfoGraphDTOs(string coinId
            , TimePeriod period
            , DateTime? startDate
            , DateTime? endDate)
        {
            if (string.IsNullOrWhiteSpace(coinId)) return null;

            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"SELECT
      [date]
      ,[price]
  FROM [dbo].[CoinApiCache]
    where coin_Id = @coin_Id 
            AND time_type = @time_type
            AND coin_graph_id = 
                (select coin_graph_id 
                    from CryptoСurrency 
                    where CryptoСurrency.Id = [CoinApiCache].coin_Id )
            AND ((@startDate is null and @endDate is null) 
				OR (date between @startDate and @endDate)
				OR (@endDate is null and date >= @startDate))
    order by date ASC", conn))
                {
                    comm.CommandType = CommandType.Text;

                    var param = comm.Parameters;
                    param.AddWithValue("@coin_Id ", coinId);
                    param.AddWithValue("@time_type", period);
                    param.AddWithValue("@startDate", (object)startDate ?? DBNull.Value);
                    param.AddWithValue("@endDate", (object)endDate ?? DBNull.Value);

                    await conn.OpenAsync();
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        var list = new List<CoinInfoGraphDTO>();
                        if (!reader.HasRows) return list;
                        while (await reader.ReadAsync())
                        {
                            list.Add(new CoinInfoGraphDTO()
                            {
                                ClosePrice = GetDouble(reader, "price"),
                                CloseTime = GetDateTime(reader, "date")
                            });
                        }

                        return list;
                    }
                }
            }
        }

        public async Task<DateTime> GetLastStartDateAndDeleteRow(string coinId
            , TimePeriod period
            , string coin_graph_id)
        {
            if (string.IsNullOrWhiteSpace(coinId)) throw new ArgumentException("coinId is null or empty");

            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"SELECT TOP (1) 
    [date]
  FROM [dbo].[CoinApiCache]
  where time_type = @period 
        AND coin_Id = @coinId 
        AND coin_graph_id = @coin_graph_id
  order by date desc;

delete [CoinApiCache] where id = (SELECT TOP (1) [Id]
  FROM [dbo].[CoinApiCache]
  where time_type = @period 
        AND coin_Id = @coinId 
        AND coin_graph_id = @coin_graph_id
  order by date desc)", conn))
                {
                    comm.CommandType = CommandType.Text;

                    var parms = comm.Parameters;
                    parms.AddWithValue("@coinId", coinId);
                    parms.AddWithValue("@period", period);
                    parms.AddWithValue("@coin_graph_id", coin_graph_id);

                    await conn.OpenAsync();
                    var result = await comm.ExecuteScalarAsync();

                    if (result == DBNull.Value || result == null)
                    {
                        return DateTime.MinValue;
                    }

                    return (DateTime)result;
                }
            }
        }
    }
}