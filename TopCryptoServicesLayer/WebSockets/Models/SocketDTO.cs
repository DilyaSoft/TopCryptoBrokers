using System.Net.WebSockets;
using System.Threading;

namespace TopCrypto.ServicesLayer.WebSockets.Models
{
    public class SocketDTO
    {
        public WebSocket Socket { get; set; }

        private int _countOfCoinPrice = 10;
        public int CountOfCoinPrice
        {
            get { return Interlocked.CompareExchange(ref _countOfCoinPrice, -1, -1); }
            set { Interlocked.Exchange(ref _countOfCoinPrice, value <= 10 ? 10 : value); }
        }
    }
}
