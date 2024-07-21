using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinGraph.Models;
using TopCrypto.DataLayer.Services.CoinInfo.Interfaces;
using TopCrypto.DataLayer.Services.CoinInfo.Models;
using TopCrypto.DataLayer.Services.Interfaces;

namespace TopCrypto.DataLayer.Services.CoinInfo
{
    public class CoinInfoDataService : AbstractСurrencyDataService, ICoinInfoDataService
    {
        protected override string TableName => "CryptoСurrency";

        public CoinInfoDataService(IConfiguration config) : base(config) { }

        public async Task ClearAndInsertNewCryptoCurrency(CryptoCurrencyDTO[] listOfCurrency)
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                await conn.OpenAsync();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (var comm = new SqlCommand("DELETE CryptoСurrency;", conn))
                        {
                            comm.CommandType = CommandType.Text;
                            comm.Transaction = transaction;

                            await comm.ExecuteNonQueryAsync();
                        }

                        foreach (var entry in listOfCurrency)
                        {
                            using (var comm = new SqlCommand(@"INSERT INTO [dbo].[CryptoСurrency]
           ([Id]
           ,[coin_abb]
           ,[coin_graph_id])
     VALUES
           (@Id
           ,@coin_abb
           ,@coin_graph_id)", conn))
                            {
                                comm.CommandType = CommandType.Text;
                                comm.Transaction = transaction;

                                var par = comm.Parameters;
                                par.AddWithValue("@Id", entry.Id);
                                par.AddWithValue("@coin_abb", entry.Abb);
                                par.AddWithValue("@coin_graph_id", DbNullOrObject(entry.MarketId));

                                await comm.ExecuteNonQueryAsync();
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task<IList<CoinIdNameDTO>> GetAllCoinsIDFromDBExistedInDB()
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"SELECT cc.[Id]
      ,ccic.[name]
  FROM [dbo].[CryptoСurrency] cc
	left join [CryptoСurrencyIdsCache] ccic on ccic.coin_id = cc.Id", conn))
                {
                    comm.CommandType = CommandType.Text;

                    await conn.OpenAsync();
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        var list = new List<CoinIdNameDTO>();
                        if (!reader.HasRows) return list;
                        while (await reader.ReadAsync())
                        {
                            list.Add(new CoinIdNameDTO()
                            {
                                Name = GetNullOrString(reader, "name"),
                                Id = GetNullOrString(reader, "Id")
                            });
                        }

                        return list;
                    }
                }
            }
        }
    }
}
