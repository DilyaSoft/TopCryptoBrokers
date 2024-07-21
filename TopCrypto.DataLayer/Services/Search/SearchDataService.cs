using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Interfaces;
using TopCrypto.DataLayer.Services.Search.Interfaces;
using TopCrypto.DataLayer.Services.Search.Models;

namespace TopCrypto.DataLayer.Services.Search
{
    public enum SearchObjectType
    {
        News = 1,
        Broker = 2
    }

    public class SearchDataService : AbstractSQLDataService, ISearchDataService
    {
        public SearchDataService(IConfiguration config) : base(config) { }

        public async Task<List<SearchDTO>> GetSearchResult(string queryPart)
        {
            IConfigurationSection myArraySection = _config.GetSection("supportedCultures");
            var culturesEnum = myArraySection.AsEnumerable().Where(x => !string.IsNullOrWhiteSpace(x.Value));

            var searchDTOs = new List<SearchDTO>();

            queryPart = queryPart.Replace("[", "[[]");
            queryPart = queryPart.Replace("_", "[_]");
            queryPart = queryPart.Replace("%", "[%]");
            queryPart = string.Format("%{0}%", queryPart.Trim(' '));

            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                var tableCheck = @"if( not exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = @tableNameBroker{0}))
                        begin
	                        THROW 51000, 'The table does not exist.', 1;  
                        end;

                    if( not exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = @tableNameNews{0}))
                        begin
	                        THROW 51000, 'The table does not exist.', 1;  
                        end;";

                var tableCheckPart = new StringBuilder();

                var selectPart = new StringBuilder();

                using (SqlCommand comm = new SqlCommand())
                {
                    comm.Connection = conn;
                    var commParams = comm.Parameters;
                    commParams.AddWithValue("@part", queryPart);
                    var i = 0;
                    foreach (var culture in culturesEnum)
                    {
                        tableCheckPart.AppendLine(string.Format(tableCheck, i));

                        commParams.AddWithValue("@tableNameBroker" + i, "Brokers." + culture.Value);
                        commParams.AddWithValue("@tableNameNews" + i, "News." + culture.Value);

                        selectPart.AppendLine(string.Format(@"Select id [Id], title [Title], 1 [Type] from [News.{0}]
	                where isHidden = 0 and (previewBody like @part 
		                or body like @part 
		                or title like @part)
                Union all
                Select loc.Id [Id], Name [Title], 2 [Type] from [Brokers.{0}] loc
	                join Brokers br on br.id = loc.Id
	                where loc.IsHidden = 0 and (loc.Name like @part 
		                or loc.Description like @part)
                Union all", culture.Value));

                        i++;
                    }

                    var selectPartStr = selectPart.ToString();

                    comm.CommandText = tableCheckPart.ToString() + " " + selectPartStr.Substring(0, selectPartStr.Length - 11);


                    await conn.OpenAsync();

                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows) return searchDTOs;

                        while (await reader.ReadAsync())
                        {
                            searchDTOs.Add(new SearchDTO()
                            {
                                Id = reader.GetInt32(0),
                                FoundedPart = reader.GetString(1),
                                Type = (SearchObjectType)reader.GetInt32(2)
                            });
                        }
                    }
                }
            }
            return searchDTOs;
        }
    }
}
