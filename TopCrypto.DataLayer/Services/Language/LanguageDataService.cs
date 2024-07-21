using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Interfaces;
using TopCrypto.DataLayer.Services.Language.Interfaces;
using TopCrypto.DataLayer.Services.Language.Models;

namespace TopCrypto.DataLayer.Services.Language
{
    public class LanguageDataService : AbstractSQLDataService, ILanguageDataService
    {
        public LanguageDataService(IConfiguration config) : base(config) { }

        public async Task<IList<LanguageDTO>> GetLanguageList()
        {
            var languageDTOs = new List<LanguageDTO>();
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (var comm = new SqlCommand(@"SELECT [Name], [Culture], [LanguageSelectorImage]
  FROM [dbo].[Languages]", conn))
                {
                    await conn.OpenAsync();

                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows) return languageDTOs;

                        while (await reader.ReadAsync())
                        {
                            languageDTOs.Add(new LanguageDTO()
                            {
                                Name = GetNullOrString(reader, "Name"),
                                Culture = GetNullOrString(reader, "Culture"),
                                Image = GetNullOrString(reader, "LanguageSelectorImage")
                            });
                        }
                    }
                }
            }
            return languageDTOs;
        }
    }
}
