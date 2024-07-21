using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinIds.Interfaces;
using TopCrypto.DataLayer.Services.CoinIds.Models;
using TopCrypto.DataLayer.Services.Interfaces;

namespace TopCrypto.DataLayer.Services.CoinIds
{
    public class CoinIdsDataService : AbstractSQLDataService, ICoinIdsDataService
    {
        public CoinIdsDataService(IConfiguration config) : base(config)
        {
        }

        public async Task<IList<CoinIdsDTO>> GetAllCoinsIDFromDB()
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"SELECT [Id]
      ,[name]
      ,[symbol]
      ,[coin_id]
  FROM [dbo].[CryptoСurrencyIdsCache]", conn))
                {
                    comm.CommandType = CommandType.Text;

                    await conn.OpenAsync();
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        var list = new List<CoinIdsDTO>();
                        if (!reader.HasRows) return list;
                        while (await reader.ReadAsync())
                        {
                            list.Add(new CoinIdsDTO()
                            {
                                Name = GetNullOrString(reader, "name"),
                                Symbol = GetNullOrString(reader, "symbol"),
                                Id = GetNullOrString(reader, "coin_id")
                            });
                        }

                        return list;
                    }
                }
            }
        }

        public async Task ClearAndInsertNewCache(IList<CoinIdsDTO> listDTOs)
        {
            if (listDTOs == null || listDTOs.Count == 0) return;

            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                await conn.OpenAsync();
                using (var trans = conn.BeginTransaction())
                {
                    using (var comm = conn.CreateCommand())
                    {
                        comm.Transaction = trans;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = @"delete [dbo].[CryptoСurrencyIdsCache];";

                        await comm.ExecuteNonQueryAsync();
                    }

                    using (var comm = conn.CreateCommand())
                    {
                        comm.Transaction = trans;
                        comm.CommandType = CommandType.Text;
                        comm.CommandText = @"INSERT INTO [dbo].[CryptoСurrencyIdsCache]
           ([name]
           ,[symbol]
           ,[coin_id])
     VALUES
           (@name
           ,@symbol
           ,@coin_id)";
                        var param = comm.Parameters;
                        param.Add(new SqlParameter("@coin_id", SqlDbType.NVarChar));
                        param.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                        param.Add(new SqlParameter("@symbol", SqlDbType.NVarChar));
                        try
                        {
                            foreach (var itemDTO in listDTOs)
                            {
                                param[0].Value = itemDTO.Id;
                                param[1].Value = itemDTO.Name;
                                param[2].Value = itemDTO.Symbol;
                                if (await comm.ExecuteNonQueryAsync() != 1)
                                {
                                    //'handled as needed, 
                                    //' but this snippet will throw an exception to force a rollback
                                    throw new Exception("exception");
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
