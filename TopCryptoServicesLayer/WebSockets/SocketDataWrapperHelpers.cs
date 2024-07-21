using System;
using TopCrypto.ServicesLayer.WebSockets.Interfaces;

namespace TopCrypto.ServicesLayer.WebSockets
{
    public class SocketDataWrapperHelpers : ISocketDataWrapperHelpers
    {
        public string WrapForCoinData(string jsonData, int countOfPrices, string grapthNodes)
        {
            return String.Format("{{\"priceCoint\":{0}, \"countOfPrices\":{1}, \"grapthNodes\":{2}}}"
                , jsonData
                , countOfPrices
                , grapthNodes);
        }

        public string WrapForFiatCurency(string jsonData)
        {
            return String.Format("{{\"fiatCurency\":{0}}}", jsonData);
        }
    }
}
