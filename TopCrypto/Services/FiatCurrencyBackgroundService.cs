using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TopCrypto.ServicesLayer.DBLogging.Interfaces;
using TopCrypto.ServicesLayer.FiatCurrency.Interfaces;

namespace TopCrypto.Services
{
    public class FiatCurrencyBackgroundService : BackgroundService
    {
        private IFiatCurrencyBackroundHelper _fiatCurrencyBackroundHelper;
        private IDBLoggingService _dbLoggingService;
        private readonly ILogger<FiatCurrencyBackgroundService> _logger;

        public FiatCurrencyBackgroundService(IDBLoggingService dbLoggingService
            , IFiatCurrencyBackroundHelper fiatCurrencyBackroundHelper
            , ILogger<FiatCurrencyBackgroundService> logger)
        {
            this._dbLoggingService = dbLoggingService;
            this._fiatCurrencyBackroundHelper = fiatCurrencyBackroundHelper;
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
                        await _fiatCurrencyBackroundHelper.Execute();

                        await Task.Delay(1000 * 60 * 60);//1 hour
                    }
                    catch (Exception e)
                    {
                       // await _dbLoggingService.WriteLog(e, "Working with sockets FiatCurrency");
                        _logger.LogError(e, "Working with sockets FiatCurrency");
                        await Task.Delay(1000 * 60 * 60);
                    }
                }
            });
        }
    }
}
