using System.Threading.Tasks;
using TopCrypto.DataLayer.Services.Broker.Models;

namespace TopCrypto.DataLayer.Services.Broker.Interfaces
{
    public interface IBrokerDataService
    {
        Task RemoveBroker(int id);
        Task<BrokerListingDTO> GetTopBrokers(string cultureName);
        Task<BrokerDTO> GetBrokerById(string name, string cultureName);
        Task<string> GetJsonBrokerListForAdmin();
        Task AddBrokerForAdmin(BrokerAdminDTO model);
        Task<BrokerLocalizationDTO> GetBrokerLocalization(int id, string cultureName);
        Task UpdateBrokerLocalization(int id, string cultureName, BrokerLocalizationDTO dto);
        Task<BrokerAdminDTO> GetBrokerAdminById(int id);
        Task UpdateBroker(int id, BrokerAdminDTO dto);
    }
}