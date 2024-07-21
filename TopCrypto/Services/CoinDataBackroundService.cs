using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TopCrypto.ServicesLayer.CoinInfo.Interfaces;
using TopCrypto.ServicesLayer.DBLogging.Interfaces;

namespace TopCrypto.Services
{
    public class CoinDataBackroundService : BackgroundService
    {
        private readonly ICoinInfoBackroundHelper _coinDataBackroundHelper;
        private readonly IDBLoggingService _dbLoggingService;
        private readonly ILogger<CoinDataBackroundService> _logger;

        public CoinDataBackroundService(ICoinInfoBackroundHelper coinDataBackroundHelper
            , IDBLoggingService dbLoggingService
            , ILogger<CoinDataBackroundService> logger)
        {
            this._coinDataBackroundHelper = coinDataBackroundHelper;
            this._dbLoggingService = dbLoggingService;
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
                        await _coinDataBackroundHelper.Execute();
                        await Task.Delay(5 * 60 * 1000 + 30 * 1000);
                    }
                    catch (Exception e)
                    {
                         await _dbLoggingService.WriteLog(e, "Working with sockets CoinData");
                        _logger.LogError(e, "Working with sockets CoinData");

                        await Task.Delay(10 * 60 * 1000);
                    }
                }
            });
        }

    }
}
