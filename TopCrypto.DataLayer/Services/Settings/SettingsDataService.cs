using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.CoinGraph;
using TopCrypto.DataLayer.Services.Interfaces;
using TopCrypto.DataLayer.Services.Settings.Interfaces;
using TopCrypto.DataLayer.Services.Settings.Models;

namespace TopCrypto.DataLayer.Services.Settings
{
    public class SettingsDataService : AbstractSQLDataService, ISettingsDataService
    {
        public SettingsDataService(IConfiguration config) : base(config)
        {
        }

        public async Task<SettingDTO> GetSettingByIdQuery(string id, string query)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id is null or empty");

            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"
            SELECT [id]
                , [value]
                , [query]
                FROM [dbo].[Settings]
                where id = @id AND 
                    ((query is null and @query is null) OR query = @query);", conn))
                {
                    comm.CommandType = CommandType.Text;
                    var param = comm.Parameters;
                    param.AddWithValue("@id", id);
                    param.AddWithValue("@query", DbNullOrObject(query));

                    await conn.OpenAsync();

                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows) return null;
                        await reader.ReadAsync();

                        return new SettingDTO()
                        {
                            Id = GetNullOrString(reader, "id"),
                            Value = GetNullOrString(reader, "value"),
                            Query = GetNullOrString(reader, "query")
                        };
                    }
                }
            }
        }

        public async Task<SettingDTO> GetSettingByIdQueryWithDefaultEnglish(string id, string query)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id is null or empty");

            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"
            select top(1) * from (
                SELECT [id]
	                , [value]
	                , [query]
	                FROM [dbo].[Settings]
		                where id = @id AND 
                                value is not null AND
		                ((query is null and @query is null) OR query = @query)
                union ALL
                SELECT [id]
	                , [value]
	                , [query]
	                FROM [dbo].[Settings]
		                where id = @id AND query = 'en-US') setting;", conn))
                {
                    comm.CommandType = CommandType.Text;
                    var param = comm.Parameters;
                    param.AddWithValue("@id", id);
                    param.AddWithValue("@query", DbNullOrObject(query));

                    await conn.OpenAsync();

                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows) return null;
                        await reader.ReadAsync();

                        return new SettingDTO()
                        {
                            Id = GetNullOrString(reader, "id"),
                            Value = GetNullOrString(reader, "value"),
                            Query = GetNullOrString(reader, "query")
                        };
                    }
                }
            }
        }


        public async Task<IList<SettingDTO>> GetSettingByListOfIdQueryWithDefaultEnglish(IList<string> ids, string query)
        {
            if (ids.Count == 0 || ids.Any(x => string.IsNullOrWhiteSpace(x)))
            {
                throw new ArgumentException("id is null or empty");
            }

            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                var commandPattern = @"
            select top(1) * from (
                SELECT [id]
	                , [value]
	                , [query]
	                FROM [dbo].[Settings]
		                where id = @id{0} AND 
                            value is not null AND
		                ((query is null and @query is null) OR query = @query)
                union ALL
                SELECT [id]
	                , [value]
	                , [query]
	                FROM [dbo].[Settings]
		                where id = @id{0} AND query = 'en-US') setting";

                using (var comm = new SqlCommand())
                {
                    var i = 0;
                    var commandList = new List<string>();
                    var param = comm.Parameters;
                    param.AddWithValue("@query", DbNullOrObject(query));

                    foreach (var id in ids)
                    {
                        commandList.Add(string.Format(commandPattern, i));
                        param.AddWithValue("@id" + i, id);

                        i++;
                    }

                    comm.CommandType = CommandType.Text;
                    comm.CommandText = string.Join("   UNION ALL   ", commandList);

                    comm.Connection = conn;

                    await conn.OpenAsync();

                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        var list = new List<SettingDTO>();
                        if (!reader.HasRows) return list;
                        while (await reader.ReadAsync())
                        {
                            list.Add(new SettingDTO()
                            {
                                Id = GetNullOrString(reader, "id"),
                                Value = GetNullOrString(reader, "value"),
                                Query = GetNullOrString(reader, "query")
                            });
                        }

                        return list;
                    }
                }
            }
        }

        public async Task AddOrUpdateSetting(string id, string value, string query)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id is null or empty");

            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(
@"
BEGIN TRY  
    BEGIN TRANSACTION; 

    if not exists(select * from Settings where id = @id and ((query is null and @query is null) OR query = @query))
    BEGIN
        insert settings(id, value, query)
	            values(@id, @value, @query)
    END
    ELSE
    BEGIN
        UPDATE [dbo].[Settings]
            set value = @value
                where id = @id and ((query is null and @query is null) OR query = @query);
    END

    COMMIT TRANSACTION;
END TRY  
BEGIN CATCH
    ROLLBACK TRANSACTION;
	THROW;
END CATCH;", conn))
                {
                    comm.CommandType = CommandType.Text;

                    var param = comm.Parameters;
                    param.AddWithValue("@id", id);
                    param.AddWithValue("@value", DbNullOrObject(value));
                    param.AddWithValue("@query", DbNullOrObject(query));

                    await conn.OpenAsync();
                    await comm.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateCoinSettings(string coinId, TimePeriod? timeType)
        {
            if (coinId != null && string.IsNullOrEmpty(coinId)) throw new ArgumentException("coinId is empty");
            if (coinId != null && timeType == null
                || coinId == null && timeType != null) throw new ArgumentException("logic error");

            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(
@"
BEGIN TRY  
    BEGIN TRANSACTION; 

    if not exists(select * from Settings where id = 'last_saved_coin_id')
    BEGIN
        insert settings(id, value)
	            values('last_saved_coin_id', @coin_Id)
    END
    ELSE
    BEGIN
        UPDATE [dbo].[Settings]
            set value = @coin_Id
                where id = 'last_saved_coin_id';
    END


    if not exists(select * from Settings where id = 'last_time_type')
    BEGIN
        insert settings(id, value)
	            values('last_time_type', @time_type)
    END
    ELSE
    BEGIN
        UPDATE [dbo].[Settings]
            set value = @time_type
                where id = 'last_time_type';
    END


    COMMIT TRANSACTION;
END TRY  
BEGIN CATCH
    ROLLBACK TRANSACTION;
	THROW;
END CATCH;", conn))
                {
                    comm.CommandType = CommandType.Text;

                    var param = comm.Parameters;
                    param.AddWithValue("@coin_Id", DbNullOrObject(coinId));
                    param.AddWithValue("@time_type", DbNullOrObject(timeType));

                    await conn.OpenAsync();
                    await comm.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<CoinSettingDTO> GetCoinSetting()
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"SELECT 
        (select value from settings where id = 'last_saved_coin_id') [last_saved_coin_id]
        ,(select value from settings where id = 'last_time_type') [last_time_type]", conn))
                {
                    comm.CommandType = CommandType.Text;

                    await conn.OpenAsync();
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows) return new CoinSettingDTO();

                        await reader.ReadAsync();
                        return new CoinSettingDTO()
                        {
                            LastSavedCoinId = GetNullOrString(reader, "last_saved_coin_id"),
                            LastTimeType = GetNullableInt(reader, "last_time_type")
                        };
                    }
                }
            }
        }

        #region Is Fresh
        public async Task<bool> IsCacheFresh()
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"
	SELECT count(*)
      FROM [dbo].[Settings]
	    where 
            id = @id            
            and ISDATE(value) = 1
		    and DATEDIFF(dd, CONVERT(datetime, value), getdate()) is not null
		    and DATEDIFF(dd, CONVERT(datetime, value), getdate()) = 0
            and not exists (select * from settings where id = 'last_saved_coin_id' and value is not null)
		    and not exists (select * from settings where id = 'last_time_type' and value is not null);", conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id", "coin_api_cache_updated_time");

                    await conn.OpenAsync();
                    var result = await comm.ExecuteScalarAsync();

                    return result != DBNull.Value
                        && result != null
                        && Int32.TryParse(result.ToString(), out int count)
                        && count > 0;
                }
            }
        }

        public async Task<bool> IsMarketCacheFresh()
        {
            return await CheckIsDateIsTodayInSettings("coin_api_cache_market_updated_time");
        }

        public async Task<bool> IsIdsCacheFresh()
        {
            return await CheckIsDateIsTodayInSettings("coin_api_cache_ids_updated_time");
        }

        private async Task<bool> CheckIsDateIsTodayInSettings(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("invalid column name");

            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"
	SELECT count(*)
      FROM [dbo].[Settings]
	    where isDate(value) = 1
            and DATEDIFF(dd, CONVERT(datetime, value), getdate()) is not null
		    and DATEDIFF(dd, CONVERT(datetime, value), getdate()) = 0
            and id = @id;", conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id", id);

                    await conn.OpenAsync();
                    var result = await comm.ExecuteScalarAsync();

                    return result != DBNull.Value
                        && result != null
                        && Int32.TryParse(result.ToString(), out int count)
                        && count > 0;
                }
            }
        }
        #endregion

        #region Update Time
        public async Task UpdateSettingsCacheUpdateTime()
        {
            await SetFreshTimeInSettingsInSpecificColumn("coin_api_cache_updated_time");
        }

        public async Task UpdateSettingsIdsUpdateTime()
        {
            await SetFreshTimeInSettingsInSpecificColumn("coin_api_cache_ids_updated_time");
        }

        public async Task UpdateSettingsMarketUpdateTime()
        {
            await SetFreshTimeInSettingsInSpecificColumn("coin_api_cache_market_updated_time");
        }

        public async Task SetFreshTimeInSettingsInSpecificColumn(string id)
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"
    BEGIN TRY  
        BEGIN TRANSACTION; 

        if not exists(select * from settings where id = @id)
        BEGIN
	        insert settings(id, value)
	            values(@id, getDate())
        END
	    else
	    BEGIN
		    Update settings 
	                set value = getDate()
                where id = @id;
	    END

        COMMIT TRANSACTION;
    END TRY  
    BEGIN CATCH
        ROLLBACK TRANSACTION;
	    THROW;
    END CATCH;", conn))
                {
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id", id);

                    await conn.OpenAsync();
                    await comm.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion
    }
}
