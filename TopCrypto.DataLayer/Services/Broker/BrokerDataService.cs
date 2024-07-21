using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Broker.Interfaces;
using TopCrypto.DataLayer.Services.Broker.Models;
using TopCrypto.DataLayer.Services.Interfaces;

namespace TopCrypto.DataLayer.Services.Broker
{
    public class BrokerDataService : AbstractSQLDataService, IBrokerDataService
    {
        private const int CULTURE_LENGTH = 7;

        public BrokerDataService(IConfiguration config) : base(config)
        {
        }

        public async Task<BrokerListingDTO> GetTopBrokers(string cultureName)
        {
            BrokerListingDTO brokerDTO = new BrokerListingDTO();

            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(
                  @"if( not exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = @tableName))
                        begin
	                        THROW 51000, 'The table does not exist.', 1;  
                        end;

select (select count(*) FROM [dbo].[Brokers." + cultureName + @"] brl
                  left join Brokers br on br.Id = brl.ID 
				  where Visible = 1) + 
	   (select count(*) FROM [dbo].[Brokers.en-US] brl
                  left join Brokers br on br.Id = brl.ID 
				  where Visible = 1 
                    and brl.id not in (select brl.ID FROM [dbo].[Brokers." + cultureName + @"] brl
					    left join Brokers br on br.Id = brl.ID 
					    where Visible = 1)) [Count];

              select * from (SELECT br.OriginalName
	                ,[UrlImg] 
	                ,[Description]
	                , brl.[Name]
	                , Mark
	                , MinDepositListing
	                , RegulationListing
	                , SpreadsListing
	                , (select CryptoId as cryptoId, 
                        value from BrokersCrypto 
                        where BrokerId = brl.[Id] 
                            FOR JSON PATH) as Cryptos 
                  FROM [dbo].[Brokers." + cultureName + @"] brl
                  left join Brokers br on br.Id = brl.ID 
                    where Visible = 1
				union all 
				SELECT br.OriginalName
	                ,[UrlImg] 
	                ,[Description]
	                , brl.[Name]
	                , Mark
	                , MinDepositListing
	                , RegulationListing
	                , SpreadsListing
	                , (select CryptoId as cryptoId, 
                        value from BrokersCrypto 
                        where BrokerId = brl.[Id] 
                            FOR JSON PATH) as Cryptos 
                  FROM [dbo].[Brokers.en-US] brl
                  left join Brokers br on br.Id = brl.ID 
                    where Visible = 1 AND br.id not in (SELECT brl.[Id] 
							  FROM [dbo].[Brokers." + cultureName + @"] brl
							  left join Brokers br on br.Id = brl.ID 
								where Visible = 1)
				) zz
				order by mark desc", conn))
                {
                    var commParams = comm.Parameters;
                    commParams.AddWithValue("@tableName", "Brokers." + cultureName);
                    await conn.OpenAsync();

                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows) return brokerDTO;
                        if (await reader.ReadAsync())
                        {
                            brokerDTO.CountVisibleTopBrokers = GetNullableInt(reader, "Count") ?? 0;
                        }

                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var newBorker = new BrokerTopDTO()
                                {
                                    BrokerName = GetNullOrString(reader, "OriginalName"),
                                    UrlImg = GetNullOrString(reader, "UrlImg"),
                                    Description = GetNullOrString(reader, "Description"),
                                    Name = GetNullOrString(reader, "Name"),
                                    Mark = GetNullableInt(reader, "Mark"),
                                    MinDepositListing = GetNullOrString(reader, "MinDepositListing"),
                                    RegulationListing = GetNullOrString(reader, "RegulationListing"),
                                    SpreadsListing = GetNullOrString(reader, "SpreadsListing")
                                };
                                var cryptos = GetNullOrString(reader, "Cryptos");
                                if (cryptos != null)
                                {
                                    newBorker.Cryptos = JsonConvert.DeserializeObject<CryptoDTO[]>(cryptos);
                                }
                                brokerDTO.BrokerTopDTOList.Add(newBorker);
                            }
                        }
                    }
                }
            }
            return brokerDTO;
        }

        public async Task RemoveBroker(int id)
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"DELETE FROM [dbo].[Brokers] WHERE Id = @Id", conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);

                    await conn.OpenAsync();
                    await comm.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<BrokerDTO> GetBrokerById(string name, string cultureName)
        {
            BrokerDTO broker = null;
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(
                  @"if( not exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = @tableName))
                        begin
	                        THROW 51000, 'The table does not exist.', 1;  
                        end

                        
if(Exists(SELECT * FROM [dbo].[Brokers." + cultureName + @"] brl
	                        left join Brokers br on br.Id = brl.ID 
		                        where brl.Id = (select top 1 Id from Brokers where OriginalName = @OrigName) and Visible = 1))
BEGIN

SELECT brl.[Id]
	                        ,UrlImg
	                        ,Name
	                        ,Mark
	                        ,RegulatedBy
	                        ,Foundation
	                        ,BusinessNature

	                        ,Leverage
	                        ,Instruments
	                        ,Platforms
	                        ,AccountTypes
	                        ,LotSizes
	                        ,DemoAccount
	                        ,AccountCurrency
	                        ,PairsOffered
	                        ,PaymentMethods
	                        ,Commissions
	                        ,AdditionalText
	                        ,ShortInfo1
	                        ,ShortInfo2
	                        ,EndInfo
	                        ,PhoneNumbersHtml
	                        ,EmailHtml
	                        FROM [dbo].[Brokers." + cultureName + @"] brl
	                        left join Brokers br on br.Id = brl.ID 
		                        where brl.Id = (select top 1 Id from Brokers where OriginalName = @OrigName) and Visible = 1;
END 
ELSE 
BEGIN
	SELECT brl.[Id]
	                        ,UrlImg
	                        ,Name
	                        ,Mark
	                        ,RegulatedBy
	                        ,Foundation
	                        ,BusinessNature

	                        ,Leverage
	                        ,Instruments
	                        ,Platforms
	                        ,AccountTypes
	                        ,LotSizes
	                        ,DemoAccount
	                        ,AccountCurrency
	                        ,PairsOffered
	                        ,PaymentMethods
	                        ,Commissions
	                        ,AdditionalText
	                        ,ShortInfo1
	                        ,ShortInfo2
	                        ,EndInfo
	                        ,PhoneNumbersHtml
	                        ,EmailHtml
	                        FROM [dbo].[Brokers.en-US] brl
	                        left join Brokers br on br.Id = brl.ID 
		                        where brl.Id = (select top 1 Id from Brokers where OriginalName = @OrigName) and Visible = 1;
END", conn))
                {
                    var param = comm.Parameters;
                    param.AddWithValue("@OrigName", name);
                    param.AddWithValue("@tableName", "Brokers." + cultureName);
                    await conn.OpenAsync();

                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows) return broker;
                        await reader.ReadAsync();

                        broker = new BrokerDTO()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Mark = GetNullableInt(reader, "Mark"),
                            Foundation = GetNullableInt(reader, "Foundation"),

                            UrlImg = GetNullOrString(reader, "UrlImg"),
                            Name = GetNullOrString(reader, "Name"),
                            RegulatedBy = GetNullOrString(reader, "RegulatedBy"),
                            BusinessNature = GetNullOrString(reader, "BusinessNature"),
                            Leverage = GetNullOrString(reader, "Leverage"),

                            Instruments = GetNullOrString(reader, "Instruments"),
                            Platforms = GetNullOrString(reader, "Platforms"),
                            AccountTypes = GetNullOrString(reader, "AccountTypes"),
                            LotSizes = GetNullOrString(reader, "LotSizes"),
                            DemoAccount = GetNullOrString(reader, "DemoAccount"),

                            AccountCurrency = GetNullOrString(reader, "AccountCurrency"),
                            PairsOffered = GetNullOrString(reader, "PairsOffered"),
                            PaymentMethods = GetNullOrString(reader, "PaymentMethods"),
                            Commissions = GetNullOrString(reader, "Commissions"),
                            AdditionalText = GetNullOrString(reader, "AdditionalText"),
                            ShortInfo1 = GetNullOrString(reader, "ShortInfo1"),
                            ShortInfo2 = GetNullOrString(reader, "ShortInfo2"),
                            EndInfo = GetNullOrString(reader, "EndInfo"),
                            PhoneNumbersHtml = GetNullOrString(reader, "PhoneNumbersHtml"),
                            EmailHtml = GetNullOrString(reader, "EmailHtml")
                        };
                    }
                }
            }
            return broker;
        }

        #region Admin        
        public async Task<string> GetJsonBrokerListForAdmin()
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"DECLARE @Culture nvarchar(10),
    @SQL nvarchar(max),
    @SqlJoin nvarchar(max) = '',
    @SqlColumn nvarchar(max) = '';
DECLARE culture_cursor CURSOR Local FOR select Culture from Languages;

Open culture_cursor;
FETCH NEXT FROM culture_cursor INTO @Culture;

WHILE @@FETCH_STATUS = 0
  BEGIN

    set @Culture = REPLACE(@Culture, ' ', '');

  if( not exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = ('Brokers.'+@Culture)))
                        begin
              FETCH NEXT FROM culture_cursor INTO @Culture;
                          CONTINUE;
                        end
   

  set @SqlJoin = @SqlJoin + 'left join [Brokers.'+@Culture+'] ['+@Culture+'] on ['+@Culture+'].id = br.id ';
  set @SqlColumn = @SqlColumn + ', ['+@Culture+'].Visible as ['+@Culture+']';

    FETCH NEXT FROM culture_cursor INTO @Culture;
  END
CLOSE culture_cursor;
DEALLOCATE culture_cursor;

set @SQL = 'select * from (
select br.Id as ID, OriginalName as Name, Mark as Mark '+@SqlColumn+'
  from Brokers br '+@SqlJoin+'
    ) as temp FOR JSON AUTO'

exec(@SQL)", conn))
                {
                    await conn.OpenAsync();

                    var result = new StringBuilder();
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows) return result.ToString();
                        while (await reader.ReadAsync())
                        {
                            result.Append(reader.GetString(0));
                        }
                    }
                    return result.ToString();
                }
            }
        }

        public async Task AddBrokerForAdmin(BrokerAdminDTO model)
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"INSERT INTO [dbo].[Brokers]
           ([OriginalName]
           ,[Mark]
           ,[Foundation]
           ,[PhoneNumbersHtml]
           ,[EmailHtml])
     VALUES
           (@OriginalName
           ,@Mark
           ,@Foundation
           ,@PhoneNumbersHtml
           ,@EmailHtml)", conn))
                {
                    var param = comm.Parameters;
                    param.AddWithValue("@OriginalName", model.OriginalName);
                    param.AddWithValue("@Mark", model.Mark);
                    param.AddWithValue("@Foundation", model.Foundation);
                    param.AddWithValue("@PhoneNumbersHtml", model.PhoneNumbersHtml);
                    param.AddWithValue("@EmailHtml", model.EmailHtml);

                    await conn.OpenAsync();
                    await comm.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<BrokerLocalizationDTO> GetBrokerLocalization(int id, string cultureName)
        {
            BrokerLocalizationDTO broker = null;
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(
                  @"if( not exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = @tableName))
                        begin
	                        THROW 51000, 'The table does not exist.', 1;  
                        end

                        SELECT [Id]
                          ,[Description]
                          ,[Instruments]
                          ,[RegulatedBy]
                          ,[UrlImg]
                          ,[Name]
                          ,[Visible]
                          ,[MinDepositListing]
                          ,[RegulationListing]
                          ,[SpreadsListing]
                          ,[Regulation]
                          ,[BusinessNature]
                          ,[Leverage]
                          ,[Platforms]
                          ,[AccountTypes]
                          ,[LotSizes]
                          ,[DemoAccount]
                          ,[AccountCurrency]
                          ,[PairsOffered]
                          ,[PaymentMethods]
                          ,[Commissions]
                          ,[AdditionalText]
                          ,[ShortInfo1]
                          ,[ShortInfo2]
                          ,[EndInfo]
                  FROM [dbo].[Brokers." + cultureName + @"] brl 
                   where brl.Id = @Id", conn))
                {
                    var commPars = comm.Parameters;
                    commPars.AddWithValue("@Id", id);
                    commPars.AddWithValue("@tableName", "Brokers." + cultureName);
                    await conn.OpenAsync();

                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows) return broker;
                        await reader.ReadAsync();
                        broker = new BrokerLocalizationDTO()
                        {
                            Description = GetNullOrString(reader, "Description"),
                            Visible = GetBoolean(reader, "Visible"),
                            Instruments = GetNullOrString(reader, "Instruments"),
                            RegulatedBy = GetNullOrString(reader, "RegulatedBy"),
                            UrlImg = GetNullOrString(reader, "UrlImg"),
                            Name = GetNullOrString(reader, "Name"),
                            MinDepositListing = GetNullOrString(reader, "MinDepositListing"),
                            RegulationListing = GetNullOrString(reader, "RegulationListing"),
                            SpreadsListing = GetNullOrString(reader, "SpreadsListing"),
                            Regulation = GetNullOrString(reader, "Regulation"),
                            BusinessNature = GetNullOrString(reader, "BusinessNature"),
                            Leverage = GetNullOrString(reader, "Leverage"),
                            Platforms = GetNullOrString(reader, "Platforms"),
                            AccountTypes = GetNullOrString(reader, "AccountTypes"),
                            LotSizes = GetNullOrString(reader, "LotSizes"),
                            DemoAccount = GetNullOrString(reader, "DemoAccount"),
                            AccountCurrency = GetNullOrString(reader, "AccountCurrency"),
                            PairsOffered = GetNullOrString(reader, "PairsOffered"),
                            PaymentMethods = GetNullOrString(reader, "PaymentMethods"),
                            Commissions = GetNullOrString(reader, "Commissions"),
                            AdditionalText = GetNullOrString(reader, "AdditionalText"),
                            ShortInfo1 = GetNullOrString(reader, "ShortInfo1"),
                            ShortInfo2 = GetNullOrString(reader, "ShortInfo2"),
                            EndInfo = GetNullOrString(reader, "EndInfo")
                        };
                    }
                }
            }
            return broker;
        }

        public async Task UpdateBrokerLocalization(int id, string cultureName, BrokerLocalizationDTO dto)
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(
                  @"if( not exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = @tableName))
                        begin
	                        THROW 51000, 'The table does not exist.', 1;  
                        end;

                    UPDATE [dbo].[Brokers." + cultureName + @"]
                       SET [Description] = @Description
                          ,[Instruments] = @Instruments
                          ,[RegulatedBy] = @RegulatedBy
                          ,[UrlImg] = @UrlImg
                          ,[Name] = @Name
                          ,[Visible] = @Visible
                          ,[MinDepositListing] = @MinDepositListing
                          ,[RegulationListing] = @RegulationListing
                          ,[SpreadsListing] = @SpreadsListing
                          ,[Regulation] = @Regulation
                          ,[BusinessNature] = @BusinessNature
                          ,[Leverage] = @Leverage
                          ,[Platforms] = @Platforms
                          ,[AccountTypes] = @AccountTypes
                          ,[LotSizes] = @LotSizes
                          ,[DemoAccount] = @DemoAccount
                          ,[AccountCurrency] = @AccountCurrency
                          ,[PairsOffered] = @PairsOffered
                          ,[PaymentMethods] = @PaymentMethods
                          ,[Commissions] = @Commissions
                          ,[AdditionalText] = @AdditionalText
                          ,[ShortInfo1] = @ShortInfo1
                          ,[ShortInfo2] = @ShortInfo2
                          ,[EndInfo] = @EndInfo
                     WHERE Id = @Id", conn))
                {
                    var commParams = comm.Parameters;
                    commParams.AddWithValue("@Id", id);
                    commParams.AddWithValue("@tableName", "Brokers." + cultureName);

                    commParams.AddWithValue("@Description", DbNullOrObject(dto.Description));
                    commParams.AddWithValue("@Instruments", DbNullOrObject(dto.Instruments));
                    commParams.AddWithValue("@RegulatedBy", DbNullOrObject(dto.RegulatedBy));
                    commParams.AddWithValue("@UrlImg", DbNullOrObject(dto.UrlImg));
                    commParams.AddWithValue("@Name", DbNullOrObject(dto.Name));
                    commParams.AddWithValue("@Visible", DbNullOrObject(dto.Visible));
                    commParams.AddWithValue("@MinDepositListing", DbNullOrObject(dto.MinDepositListing));
                    commParams.AddWithValue("@RegulationListing", DbNullOrObject(dto.RegulationListing));
                    commParams.AddWithValue("@SpreadsListing", DbNullOrObject(dto.SpreadsListing));
                    commParams.AddWithValue("@Regulation", DbNullOrObject(dto.Regulation));
                    commParams.AddWithValue("@BusinessNature", DbNullOrObject(dto.BusinessNature));
                    commParams.AddWithValue("@Leverage", DbNullOrObject(dto.Leverage));
                    commParams.AddWithValue("@Platforms", DbNullOrObject(dto.Platforms));
                    commParams.AddWithValue("@AccountTypes", DbNullOrObject(dto.AccountTypes));
                    commParams.AddWithValue("@LotSizes", DbNullOrObject(dto.LotSizes));
                    commParams.AddWithValue("@DemoAccount", DbNullOrObject(dto.DemoAccount));
                    commParams.AddWithValue("@AccountCurrency", DbNullOrObject(dto.AccountCurrency));
                    commParams.AddWithValue("@PairsOffered", DbNullOrObject(dto.PairsOffered));
                    commParams.AddWithValue("@PaymentMethods", DbNullOrObject(dto.PaymentMethods));
                    commParams.AddWithValue("@Commissions", DbNullOrObject(dto.Commissions));
                    commParams.AddWithValue("@AdditionalText", DbNullOrObject(dto.AdditionalText));
                    commParams.AddWithValue("@ShortInfo1", DbNullOrObject(dto.ShortInfo1));
                    commParams.AddWithValue("@ShortInfo2", DbNullOrObject(dto.ShortInfo2));
                    commParams.AddWithValue("@EndInfo", DbNullOrObject(dto.EndInfo));

                    await conn.OpenAsync();
                    await comm.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<BrokerAdminDTO> GetBrokerAdminById(int id)
        {
            BrokerAdminDTO broker = null;
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(
                  @"SELECT [OriginalName]
                      ,[Mark]
                      ,[Foundation]
                      ,[PhoneNumbersHtml]
                      ,[EmailHtml]
                  FROM [dbo].[Brokers]
                  where id = @Id", conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);
                    await conn.OpenAsync();

                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows) return broker;
                        await reader.ReadAsync();

                        broker = new BrokerAdminDTO()
                        {
                            OriginalName = GetNullOrString(reader, "OriginalName"),
                            Mark = GetNullableInt(reader, "Mark"),
                            Foundation = GetNullableInt(reader, "Foundation"),
                            PhoneNumbersHtml = GetNullOrString(reader, "PhoneNumbersHtml"),
                            EmailHtml = GetNullOrString(reader, "EmailHtml"),
                        };
                    }
                }
            }
            return broker;
        }

        public async Task UpdateBroker(int id, BrokerAdminDTO dto)
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(
                  @"UPDATE [dbo].[Brokers]
   SET [OriginalName] = @OriginalName
      ,[Mark] = @Mark
      ,[Foundation] = @Foundation
      ,[PhoneNumbersHtml] = @PhoneNumbersHtml
      ,[EmailHtml] = @EmailHtml
 WHERE Id = @Id", conn))
                {
                    var commParams = comm.Parameters;
                    commParams.AddWithValue("@Id", id);

                    commParams.AddWithValue("@OriginalName", DbNullOrObject(dto.OriginalName));
                    commParams.AddWithValue("@Mark", DbNullOrObject(dto.Mark));
                    commParams.AddWithValue("@Foundation", DbNullOrObject(dto.Foundation));
                    commParams.AddWithValue("@PhoneNumbersHtml", DbNullOrObject(dto.PhoneNumbersHtml));
                    commParams.AddWithValue("@EmailHtml", DbNullOrObject(dto.EmailHtml));

                    await conn.OpenAsync();
                    await comm.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion
    }
}
