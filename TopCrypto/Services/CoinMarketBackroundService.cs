using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TopCrypto.ServicesLayer.CoinMarket.Interfaces;
using TopCrypto.ServicesLayer.DBLogging.Interfaces;

namespace TopCrypto.Services
{
    public class CoinMarketBackroundService : BackgroundService
    {
        private readonly IDBLoggingService _dbLoggingService;
        private readonly ICoinMarketService _coinMarketService;
        private ILogger<CoinMarketBackroundService> _logger;

        public CoinMarketBackroundService(IDBLoggingService dbLoggingService
            , ICoinMarketService coinIdsDataService
            , ILogger<CoinMarketBackroundService> logger)
        {
            this._dbLoggingService = dbLoggingService;
            this._coinMarketService = coinIdsDataService;
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
                        var isCacheFresh = await _coinMarketService.IsCacheFresh();
                        if (!isCacheFresh)
                        {
                            await _coinMarketService.UpdateDBCache();
                        }

                        await Task.Delay(60 * 60 * 1000);
                    }
                    catch (Exception e)
                    {
                        //  await _dbLoggingService.WriteLog(e, "Working with sockets CoinMarketBackroundService");

                        _logger.LogError(e, "Working with sockets CoinMarketBackroundService");
                        await Task.Delay(60 * 60 * 1000);
                    }
                }
            });
        }
    }
}
