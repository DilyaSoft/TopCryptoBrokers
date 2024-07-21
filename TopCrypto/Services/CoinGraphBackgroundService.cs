using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TopCrypto.ServicesLayer.CoinGraph.Interfaces;
using TopCrypto.ServicesLayer.DBLogging.Interfaces;

namespace TopCrypto.Services
{
    public class CoinGraphBackgroundService : BackgroundService
    {
        private readonly IDBLoggingService _dbLoggingService;
        private readonly ICoinInfoGraphService _coinInfoGraphService;
        private readonly ILogger<CoinGraphBackgroundService> _logger;

        public CoinGraphBackgroundService(IDBLoggingService dbLoggingService
            , ICoinInfoGraphService coinInfoGraphService
            , ILogger<CoinGraphBackgroundService> logger)
        {
            this._dbLoggingService = dbLoggingService;
            this._coinInfoGraphService = coinInfoGraphService;
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
                        var isCacheFresh = await _coinInfoGraphService.IsCacheFresh();
                        if (!isCacheFresh) {
                            await _coinInfoGraphService.UpdateDBCacheForAll();
                        }

                        await Task.Delay(60 * 60 * 1000);
                    }
                    catch (Exception e)
                    {
                     //   await _dbLoggingService.WriteLog(e, "Working with sockets CoinGraphBackground");
                        _logger.LogError(e, "Working with sockets CoinGraphBackground");
                        await Task.Delay(60 * 60 * 1000);
                    }
                }
            });
        }
    }
}
