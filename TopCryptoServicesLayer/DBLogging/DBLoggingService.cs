using System;
using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.DBLogging.Interfaces;
using TopCrypto.ServicesLayer.DBLogging.Interfaces;

namespace TopCrypto.ServicesLayer.DBLogging
{
    public class DBLoggingService : IDBLoggingService
    {
        private readonly IDBLoggingDataService _dBLoggingData;
        public DBLoggingService(IDBLoggingDataService dBLoggingData)
        {
            this._dBLoggingData = dBLoggingData;
        }

        public async Task WriteLog(Exception e, string actionName)
        {
            await _dBLoggingData.WriteLog(e.StackTrace, e.Message, actionName, e.GetType().ToString());
        }
    }
}
