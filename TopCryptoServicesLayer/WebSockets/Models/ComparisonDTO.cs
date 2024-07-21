using TopCrypto.ServicesLayer.SocketServices.WebSockets;

namespace TopCrypto.ServicesLayer.WebSockets.Models
{
    public interface IComparisonDTO
    {
        SocketComparisonEnum Code { get; set; }
    }
}
