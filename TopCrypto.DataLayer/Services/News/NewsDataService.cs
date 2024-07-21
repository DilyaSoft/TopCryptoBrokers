using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Interfaces;
using TopCrypto.DataLayer.Services.News.Interfaces;
using TopCrypto.DataLayer.Services.News.Models;
using UnidecodeSharpCore;

namespace TopCrypto.DataLayer.Services.News
{
    public class NewsDataService : AbstractSQLDataService, INewsDataService
    {
        public NewsDataService(IConfiguration config) : base(config) { }

        #region News
        public async Task<List<NewsTopDTO>> GetTopNews(int newsCount, string cultureName)
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(String.Format(@"
if( not exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = @tableName))
    begin
        THROW 51000, 'The table does not exist.', 1;  
    end;

SELECT top({0}) (FORMAT(nw.dateForShowing, 'yyyy-MM-dd-', 'en-US') + [newsUrl]) as newsUrl
      ,[imgLink]
      ,[title]
      , FORMAT( [dateForShowing], 'dd.MM.yyyy | HH:mm', 'en-US') as [dateForShowing]
      ,[previewBody]
  FROM [dbo].[News] nw
	left join [News.{1}] loc on loc.id = nw.id
  where isHidden = 0

order by [dateForShowing]", newsCount, cultureName), conn))
                {
                    comm.CommandType = CommandType.Text;

                    comm.Parameters.AddWithValue("@tableName", "News." + cultureName);

                    await conn.OpenAsync();
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        var list = new List<NewsTopDTO>();
                        if (!reader.HasRows) return list;
                        while (await reader.ReadAsync())
                        {
                            list.Add(new NewsTopDTO()
                            {
                                Url = GetNullOrString(reader, "newsUrl"),
                                ImgLink = GetNullOrString(reader, "imgLink"),
                                Title = GetNullOrString(reader, "title"),
                                DateForShowing = GetNullOrString(reader, "dateForShowing"),
                                PreviewBody = GetNullOrString(reader, "previewBody")
                            });
                        }

                        return list;
                    }
                }
            }
        }

        public async Task<NewsDetailDTO> GetNewsDetail(string link, string cultureName)
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(String.Format(@"
if( not exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = @tableName))
    begin
        THROW 51000, 'The table does not exist.', 1;  
    end;

SELECT nw.[id]
     ,[imgLink]
     ,[title]
     ,FORMAT([dateForShowing], 'dd.MM.yyyy | HH:mm', 'en-US') as [dateForShowing]
     ,[body]
 FROM [dbo].[News] nw
    left join [News.{0}] loc on loc.Id = nw.id
 where nw.id = (select top 1 id from (select id from [News.{0}] where [newsUrl] = @NewsUrl intersect select id from News where FORMAT([dateForShowing], 'yyyy-MM-dd', 'en-US') = @date) inter)
    and loc.isHidden = 0
", cultureName), conn))
                {
                    comm.CommandType = CommandType.Text;

                    comm.Parameters.AddWithValue("@date", link.Substring(0, 10));
                    comm.Parameters.AddWithValue("@NewsUrl", link.Substring(11));
                    comm.Parameters.AddWithValue("@tableName", "News." + cultureName);

                    await conn.OpenAsync();
                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows) return new NewsDetailDTO();
                        await reader.ReadAsync();
                        return new NewsDetailDTO()
                        {
                            Id = GetInt(reader, "id"),
                            ImgLink = GetNullOrString(reader, "imgLink"),
                            Title = GetNullOrString(reader, "title"),
                            DateForShowing = GetNullOrString(reader, "dateForShowing"),
                            Body = GetNullOrString(reader, "body")
                        };
                    }
                }
            }
        }
        #endregion

        #region Admin        
        public async Task<string> GetJsonNewsListForAdmin()
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

  if( not exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = ('News.'+@Culture)))
                        begin
              FETCH NEXT FROM culture_cursor INTO @Culture;
                          CONTINUE;
                        end

  set @SqlJoin = @SqlJoin + 'left join [News.'+@Culture+'] ['+@Culture+'] on ['+@Culture+'].id = br.id ';
  set @SqlColumn = @SqlColumn + ', ['+@Culture+'].isHidden as ['+@Culture+']';

    FETCH NEXT FROM culture_cursor INTO @Culture;
  END
CLOSE culture_cursor;
DEALLOCATE culture_cursor;

set @SQL = 'select * from (
select br.Id as ID, isNull(Note, '''')  as Note '+@SqlColumn+'
  from News br '+@SqlJoin+'
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

        public async Task AddNews(NewsAdminDTO model) 
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"INSERT INTO [dbo].[News]
           ([imgLink]
           ,[dateForShowing]
           ,[note])
     VALUES
           (@imgLink
           ,@dateForShowing
           ,@note)", conn))
                {
                    var param = comm.Parameters;
                    param.AddWithValue("@imgLink", model.ImgLink);
                    param.AddWithValue("@dateForShowing", model.DateForShowing);
                    param.AddWithValue("@note", model.Note);

                    await conn.OpenAsync();
                    await comm.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<NewsAdminDTO> GetAdminDTOById(int id)
        {
            NewsAdminDTO news = null;
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(
                  @"SELECT [id]
                      ,[imgLink]
                      ,[dateForShowing]
                      ,[date]
                      ,[note]
                  FROM [dbo].[News]
                  where id = @Id", conn))
                {
                    comm.Parameters.AddWithValue("@Id", id);
                    await conn.OpenAsync();

                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows) return news;
                        await reader.ReadAsync();

                        news = new NewsAdminDTO()
                        {
                            DateForShowing = GetDateTime(reader, "dateForShowing"),
                            ImgLink = GetNullOrString(reader, "imgLink"),
                            Note = GetNullOrString(reader, "note")
                        };
                    }
                }
            }
            return news;
        }

        public async Task UpdateNews(int id, NewsAdminDTO dto)
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(
                  @"UPDATE [dbo].[News]
   SET [imgLink] = @imgLink
      ,[dateForShowing] = @dateForShowing
      ,[note] = @note
 WHERE Id = @Id", conn))
                {
                    var commParams = comm.Parameters;
                    commParams.AddWithValue("@Id", id);

                    commParams.AddWithValue("@imgLink", dto.ImgLink);
                    commParams.AddWithValue("@note", DbNullOrObject(dto.Note));
                    commParams.AddWithValue("@dateForShowing", dto.DateForShowing);

                    await conn.OpenAsync();
                    await comm.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<NewsLocalizationDTO> GetLocalizationDTO(int id, string cultureName) 
        {
            NewsLocalizationDTO newsLocalizationDTO = null;
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(
                  @"if( not exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = @tableName))
                        begin
	                        THROW 51000, 'The table does not exist.', 1;  
                        end

                        SELECT [id]
                              ,[title]
                              ,[isHidden]
                              ,[body]
                              ,[previewBody]
                          FROM [dbo].[News." + cultureName + @"] nws
                   where nws.Id = @Id", conn))
                {
                    var commPars = comm.Parameters;
                    commPars.AddWithValue("@Id", id);
                    commPars.AddWithValue("@tableName", "News." + cultureName);
                    await conn.OpenAsync();

                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows) return newsLocalizationDTO;
                        await reader.ReadAsync();
                        newsLocalizationDTO = new NewsLocalizationDTO()
                        {
                            Body = GetNullOrString(reader, "body"),
                            IsHidden = GetBoolean(reader, "isHidden"),
                            PreviewBody = GetNullOrString(reader, "previewBody"),
                            Title = GetNullOrString(reader, "title")
                        };
                    }
                }
            }
            return newsLocalizationDTO;
        }

        public async Task UpdateLocalization(int id, string cultureName, NewsLocalizationDTO dto) 
        {
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(
                  @"if( not exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = @tableName))
                        begin
	                        THROW 51000, 'The table does not exist.', 1;  
                        end;

                    UPDATE [dbo].[News." + cultureName + @"]
                       SET [title] = @title
                          ,[isHidden] = @isHidden
                          ,[body] = @body
                          ,[previewBody] = @previewBody
                          ,[newsUrl] = @url
                     WHERE [id] = @Id;", conn))
                {
                    var commParams = comm.Parameters;
                    commParams.AddWithValue("@Id", id);
                    commParams.AddWithValue("@tableName", "News." + cultureName);

                    commParams.AddWithValue("@title", DbNullOrObject(dto.Title));
                    commParams.AddWithValue("@isHidden", DbNullOrObject(dto.IsHidden));
                    commParams.AddWithValue("@body", DbNullOrObject(dto.Body));
                    commParams.AddWithValue("@previewBody", DbNullOrObject(dto.PreviewBody));
                    commParams.AddWithValue("@url", DbNullOrObject(dto.Title.Unidecode()));

                    await conn.OpenAsync();
                    await comm.ExecuteNonQueryAsync();
                }
            }
        }
        #endregion
    }
}
