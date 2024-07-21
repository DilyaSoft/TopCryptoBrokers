using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinMarket.Interfaces;
using TopCrypto.DataLayer.Services.CoinMarket.Models;
using TopCrypto.DataLayer.Services.Interfaces;

namespace TopCrypto.DataLayer.Services.CoinMarket
{
    public class CoinMarketDataService : AbstractSQLDataService, ICoinMarketDataService
    {
        public CoinMarketDataService(IConfiguration config) : base(config)
        {
        }

        public async Task<IList<CoinMarketDTO>> GetDataFromDBCache()
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"SELECT [symbol_id] ,[asset_id_base]
  FROM [dbo].[CoinApiMarketCache]", conn))
                {
                    await conn.OpenAsync();
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        var list = new List<CoinMarketDTO>();
                        if (!reader.HasRows) return list;

                        while (await reader.ReadAsync())
                        {
                            list.Add(new CoinMarketDTO()
                            {
                                SymbolId = GetNullOrString(reader, "symbol_id"),
                                AssetIdBase = GetNullOrString(reader, "asset_id_base")
                            });
                        }
                        return list;
                    }
                }
            }
        }

        public async Task ClearAndInsertNewCache(IList<CoinMarketDTO> marketDTOs)
        {
            if (marketDTOs == null || marketDTOs.Count == 0) return;

            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    using (var comm = conn.CreateCommand())
                    {
                        comm.Transaction = trans;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = @"delete [dbo].[CoinApiMarketCache];";

                        await comm.ExecuteNonQueryAsync();
                    }

                    using (var comm = conn.CreateCommand())
                    {
                        comm.Transaction = trans;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = @"INSERT INTO [dbo].[CoinApiMarketCache]
           ([asset_id_base]
           ,[symbol_id])
     VALUES
           (@asset_id_base
           ,@symbol_id)";
                        var param = comm.Parameters;
                        param.Add(new SqlParameter("@asset_id_base", SqlDbType.NVarChar));
                        param.Add(new SqlParameter("@symbol_id", SqlDbType.NVarChar));
                        try
                        {
                            foreach (var marketDTO in marketDTOs)
                            {
                                param[0].Value = marketDTO.AssetIdBase;
                                param[1].Value = marketDTO.SymbolId;
                                if (await comm.ExecuteNonQueryAsync() != 1)
                                {
                                    //'handled as needed, 
                                    //' but this snippet will throw an exception to force a rollback
                                    throw new Exception("CoinMarketDataService.ClearAndInsertNewCache");
                                }
                            }
                            trans.Commit();
                        }
                        catch (Exception)
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }
    }
}
