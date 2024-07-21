using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace TopCrypto.DataLayer.Services.Interfaces
{
    public abstract class AbstractСurrencyDataService : AbstractSQLDataService, IСurrencyDataService
    {
        public AbstractСurrencyDataService(IConfiguration config) : base(config) { }

        protected abstract string TableName { get; }

        public async Task<string[]> GetDataBaseIds()
        {
            return await GetIdsFromTable(TableName);
        }

        public async Task SaveIdsToTable(string[] listOfnames)
        {
            await SaveIdsToTable(TableName, listOfnames);
        }

        protected async Task SaveIdsToTable(string tableName, string[] listOfnames)
        {
            if (string.IsNullOrWhiteSpace(tableName)) throw new Exception("tableName is null or empty");
            if (listOfnames == null || listOfnames.Length == 0) throw new Exception("listOfnames is null or empty");

            using (SqlConnection conn = new SqlConnection(GetDefaultConnectionString()))
            {
                using (SqlCommand comm = new SqlCommand("SaveListOfNames", conn))
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.Parameters.AddWithValue("@tblName", tableName);
                    comm.Parameters.AddWithValue("@listOfNames", string.Join(",", listOfnames));

                    await conn.OpenAsync();
                    await comm.ExecuteNonQueryAsync();
                }
            }
        }

        protected async Task<string[]> GetIdsFromTable(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName)) throw new Exception("tableName is null or empty");

            var localDbname = tableName.Replace(" ", string.Empty);
            using (var conn = new SqlConnection(GetDefaultConnectionString()))
            {
                string commandString = string.Format(@"if( not exists (Select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = @tableName))
begin
	THROW 51000, 'The table does not exist.', 1;  
end;
SELECT [Id] FROM [dbo].[{0}]", localDbname);
                using (var comm = new SqlCommand(commandString, conn))
                {
                    comm.Parameters.AddWithValue("@tableName", localDbname);
                    await conn.OpenAsync();

                    using (var reader = await comm.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows) return new string[] { };

                        var list = new List<string>();
                        while (await reader.ReadAsync())
                        {
                            list.Add(reader["Id"].ToString());
                        }
                        return list.ToArray();
                    }
                }
            }
        }
    }
}
