using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TopCrypto.ServicesLayer.CoinIds.Interfaces;
using TopCrypto.ServicesLayer.DBLogging.Interfaces;

namespace TopCrypto.Services
{
    public class CoinIdsBackroundService : BackgroundService
    {
        private readonly IDBLoggingService _dbLoggingService;
        private readonly ICoinIdsService _coinIdsService;
        private readonly ILogger<CoinIdsBackroundService> _logger;

        public CoinIdsBackroundService(IDBLoggingService dbLoggingService
            , ICoinIdsService coinIdsService
            , ILogger<CoinIdsBackroundService> logger)
        {
            this._dbLoggingService = dbLoggingService;
            this._coinIdsService = coinIdsService;
            this._logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var isCacheFresh = await _coinIdsService.IsCacheFresh();
                        if (!isCacheFresh)
                        {
                            await _coinIdsService.UpdateDBCacheForAll();
                        }

                        await Task.Delay(60 * 60 * 1000);
                    }
                    catch (Exception e)
                    {
                       // await _dbLoggingService.WriteLog(e, "Working with sockets CoinIdsDataService");
                        _logger.LogError(e, "Working with sockets CoinIdsDataService");
                        await Task.Delay(60 * 60 * 1000);
                    }
                }
            });
        }
    }
}
