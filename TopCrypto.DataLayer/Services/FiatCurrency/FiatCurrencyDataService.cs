using Microsoft.Extensions.Configuration;
using TopCrypto.DataLayer.Services.FiatCurrency.Interfaces;
using TopCrypto.DataLayer.Services.Interfaces;

namespace TopCrypto.DataLayer.Services.FiatCurrency
{
    public class FiatCurrencyDataService : AbstractСurrencyDataService, IFiatCurrencyDataService
    {
        protected override string TableName => "MonetaryCurrency";        

        public FiatCurrencyDataService(IConfiguration config) : base(config)
        {
        }
    }
}
