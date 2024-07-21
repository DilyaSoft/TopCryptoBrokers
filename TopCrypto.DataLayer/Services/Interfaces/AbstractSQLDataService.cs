using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace TopCrypto.DataLayer.Services.Interfaces
{
    public abstract class AbstractSQLDataService
    {
        protected IConfiguration _config;

        public AbstractSQLDataService(IConfiguration config)
        {
            this._config = config;
        }

        protected string GetDefaultConnectionString()
        {
            return _config.GetConnectionString("DefaultConnection");
        }

        protected string GetNullOrString(SqlDataReader reader, string name)
        {
            return DBNull.Value.Equals(reader[name]) ? null : Convert.ToString(reader[name]);
        }

        protected bool GetBoolean(SqlDataReader reader, string name)
        {
            return Convert.ToBoolean(reader[name]);
        }

        protected double GetDouble(SqlDataReader reader, string name)
        {
            return Convert.ToDouble(reader[name]);
        }

        protected int GetInt(SqlDataReader reader, string name)
        {
            return Convert.ToInt32(reader[name]);
        }

        protected int? GetNullableInt(SqlDataReader reader, string name)
        {
            if (reader[name] == DBNull.Value) return null;
            return Convert.ToInt32(reader[name]);
        }

        protected Object DbNullOrObject(Object obj)
        {
            return obj ?? DBNull.Value;
        }

        protected DateTime GetDateTime(SqlDataReader reader, string name)
        {
            return Convert.ToDateTime(reader[name]);
        }
    }
}
